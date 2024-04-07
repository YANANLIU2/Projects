#include <iostream>
#include <SDL_image.h>
#include <cmath> 
#include "Tile.h"
#include "CharacterBase.h"

CharacterBase::CharacterBase(SDL_Renderer* pRenderer, b2World* pWorld, EGameObjectType type)
	: GameObject(type, { 2,3 }, {0,0}, pWorld, ETileType::kDefault)
{
	Init_animations();
	m_pRenderer = pRenderer;
	m_currentFrameIndex = 0;
	m_state = EState::kIdle;
	m_direction = EMove::None;
	m_lastdirection = EMove::None;
	m_isMovingDown = m_isMovingLeft = m_isMovingRight = m_isMovingUp = false;
	m_frameTimer = 0;
	m_DeltaMovement = { 0,0 };
	m_CollisionBug = false;
}

void CharacterBase::Init_animations()
{
	for (int animIndex = 0; animIndex < int(EAnimType::Num_Types); ++animIndex)
	{
		Animation anim;
		anim.m_frameDuration = 0.1f;

		anim.m_shouldLoop = true;

		for (int frameIndex = 0; frameIndex < 4; ++frameIndex)
		{
			Frame frame;
			frame.m_src = SDL_Rect{ frameIndex * 32, animIndex * 48, 32, 48 };

			anim.m_frames.push_back(frame);
		}

		m_animMap.emplace((EAnimType)animIndex, anim);
	}
}

void CharacterBase::LoadATexture(const char * filepath)
{
	m_filepath = filepath;
	if (m_pRenderer)
	{
		if (m_pTexture)
		{
			m_pTexture = nullptr;
		}

		m_pTexture = IMG_LoadTexture(m_pRenderer, filepath);

	}
	if (!m_pTexture)
		std::cout << "Failed to load Object \n";
}

CharacterBase::~CharacterBase()
{
	SDL_DestroyTexture(m_pTexture);
	m_pBody->DestroyFixture(m_pBody->GetFixtureList());
}

void CharacterBase::Draw()
{
	SDL_Rect* pSrc = &m_animMap[m_currentAnim].m_frames[m_currentFrameIndex].m_src;
	SDL_RenderCopy(m_pRenderer, m_pTexture, pSrc, &m_dest);
}

void CharacterBase::Move(float deltaSeconds)
{
	if (m_isMovingUp && m_isMovingDown)
	{
		m_speed.y = 0;
	}
	else if (m_isMovingUp == true)
	{
		m_speed.y -= m_acceleration;
	}
	else if (m_isMovingDown == true)
	{
		m_speed.y += m_acceleration;
	}
	else
	{
		m_speed.y = 0;
	}


	if (m_isMovingLeft && m_isMovingRight)
	{
		m_speed.x = 0;
	}
	else if (m_isMovingLeft == true)
	{
		m_speed.x -= m_acceleration;
	}
	else if (m_isMovingRight == true)
	{
		m_speed.x += m_acceleration;
	}
	else
	{
		m_speed.x = 0;
	}


	if (m_speed.x > m_maxspeed)
		m_speed.x = m_maxspeed;
	else if (m_speed.x < -m_maxspeed)
		m_speed.x = -m_maxspeed;
	if (m_speed.y > m_maxspeed)
		m_speed.y = m_maxspeed;
	else if (m_speed.y < -m_maxspeed)
		m_speed.y = -m_maxspeed;

	m_DeltaMovement.x = m_speed.x * deltaSeconds;
	m_DeltaMovement.y = m_speed.y * deltaSeconds;

	m_position.x += m_DeltaMovement.x;
	m_position.y += m_DeltaMovement.y;
}

//user input will go to this function to influence the moving of the player
void CharacterBase::MoveDir(EMove direction)
{	
	m_lastdirection = m_direction;
	m_direction = direction;

	switch (direction)
	{
	case EMove::Left:
		m_isMovingLeft = true;
		break;
	case EMove::Right:
		m_isMovingRight = true;
		break;
	case EMove::Up:
		m_isMovingUp = true;
		break;
	case EMove::Down:
		m_isMovingDown = true;
		break;
	case EMove::StopUp:
		m_isMovingUp = false;
		break;
	case EMove::StopDown:
		m_isMovingDown = false;
		break;
	case EMove::StopLeft:
		m_isMovingLeft = false;
		break;
	case EMove::StopRight:
		m_isMovingRight = false;
		break;
	default:
		std::cout << "Invalid direction.\n";
		break;
	}

	//if 2 directions: up and right
	//will show walkright 
	if (m_isMovingRight)
		m_direction = EMove::Right;
	else if (m_isMovingLeft)
		m_direction = EMove::Left;
	else if (m_isMovingDown)
		m_direction = EMove::Down;
	else if (m_isMovingUp)
		m_direction = EMove::Up;
	else
		m_direction = EMove::None;

	if (m_direction != m_lastdirection)
	{
		StateCheck();
		NextAnimation();

	}

}

//when m_direction != m_lastdirection, change the animtype based on m_direction;
//make all anim_index 0 
void CharacterBase::NextAnimation()
{
	m_currentFrameIndex = 0;
	m_frameTimer = 0;

	switch (m_direction)
	{
	case EMove::Down:
		m_currentAnim = EAnimType::kWalkDown;
		break;

	case EMove::Up:
		m_currentAnim = EAnimType::kWalkUp;
		break;

	case EMove::Right:
		m_currentAnim = EAnimType::kWalkRight;
		break;

	case EMove::Left:
		m_currentAnim = EAnimType::kWalkLeft;
		break;

	default:
		break;
	}
}

//m_drection = none => idle state; other : walk state;
void CharacterBase::StateCheck()
{
	if (m_direction == EMove::None)
	{
		m_state = EState::kIdle;
		m_speed.x = 0;
		m_speed.y = 0;
	}
	else
	{
		m_state = EState::kWalk;
	}
}

void CharacterBase::OutofBox()
{
	if (m_position.x + m_size.x > WINDOW_WIDTH)
		m_position.x = WINDOW_WIDTH - m_size.x;
	if (m_position.x < 0)
		m_position.x = 0;
	if (m_position.y + m_size.y > WINDOW_HEIGHT)
		m_position.y = WINDOW_HEIGHT - m_size.y;
	if (m_position.y < 0)
		m_position.y = 0;
}

//every frame. will update animation
void CharacterBase::UpdateAnimation(float deltaSeconds)
{
	StateCheck();
	if (m_state == EState::kIdle)
		return;

	Animation* pCurrentAnim;

	pCurrentAnim = &m_animMap[m_currentAnim];
	m_frameTimer += deltaSeconds;

	if (m_frameTimer >= pCurrentAnim->m_frameDuration)
	{
		// GoTo Next frame!
		m_frameTimer -= pCurrentAnim->m_frameDuration;
		++m_currentFrameIndex;

		if (pCurrentAnim->m_shouldLoop)
		{
			m_currentFrameIndex %= pCurrentAnim->m_frames.size();
			
		}
	}
}

