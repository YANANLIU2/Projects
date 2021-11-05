#include <iostream>

#include "Map.h"
#include "Player.h"
#include "SoundManager.h"
#include <math.h>
Player::Player(SDL_Renderer* pRenderer, ECharacterImageType type, b2World* pWorld) :CharacterBase(pRenderer, pWorld, EGameObjectType::kPlayer)
{
	m_ImageType = type;
	LoadATexture("Assets/Character/001-Fighter01.png");
}


Player::~Player()
{
	SDL_DestroyTexture(m_pTexture);
}

void Player::Init()
{
	m_position.x = 300;
	m_position.y = 100;
	m_size.x = 80;
	m_size.y = 100;
	m_WidthHeight = { 2,3 };
	m_dest = { int(m_position.x),int(m_position.y) ,int(m_size.x),int(m_size.y) };
	m_speed.x = 0;
	m_speed.y = 0;
	m_acceleration = float(20);
	m_maxspeed = 200;
	m_hp = 100;
}

void Player::Update(float deltaSeconds)
{
	if (!this->IsDead())
	{
		OutofBox();
		Move(deltaSeconds);
		m_dest = { int(m_position.x), int(m_position.y), int(m_size.x), int(m_size.y) };
		UpdateAnimation(deltaSeconds);
		
		//walk sound
		if (m_state == EState::kWalk)
		{
			SoundManager::Get().Play(SoundType::Walk);
		}

		//box2D position
		m_pBody->SetTransform({ (m_position.x + m_size.x / 2.f) / kPixelsPerMeter ,(m_position.y + m_size.y / 2.f) / kPixelsPerMeter }, m_pBody->GetAngle());
	}
		
	
}

void Player::OnTriggerEnter(b2Fixture * pOther)
{
}

void Player::OnCollisionEnter(b2Fixture * pOther)
{
	// if the block is on the right or left : disable x move
	// if the block is on the up or bottom: disable y move

	if (GameObject* block = static_cast<GameObject*>(pOther->GetBody()->GetUserData()))
	{
		
		if (block->GetType() == EGameObjectType::kBlock)
		{
			//Vector2 character_MaxCorner = { m_position.x + m_size.x, m_position.y + m_size.y };
			//intpair block_MaxCorner = { block->GetLeftCorner().x + block->GetWidthHeight().x, block->GetLeftCorner().y + block->GetWidthHeight().y };
			//intpair block_MinCorner = { block->GetLeftCorner().x , block->GetLeftCorner().y};
			Vector2 block_HalfWidthHeight = { block->GetWidthHeight().x / 2.f , block->GetWidthHeight().y / 2.f };
			Vector2 block_Center = { block->GetLeftCorner().x + block_HalfWidthHeight.x,  block->GetLeftCorner().y + block_HalfWidthHeight.y };
			Vector2 character_Center = { m_position.x + m_size.x / 2.f, m_position.y + m_size.y / 2.f };
	
			//bool IsAbove = (this->m_position.y < block_MinCorner.y);
			//bool IsRight = (character_MaxCorner.x > block_MaxCorner.x);
			//bool IsBelow = (character_MaxCorner.y > block_MaxCorner.y);
			//bool IsLeft = (this->m_position.x < block_MinCorner.x);

			bool IsAbove = (character_Center.y < block_Center.y);
			bool IsRight = (character_Center.x > block_Center.x);
			bool IsBelow = (character_Center.y > block_Center.y);
			bool IsLeft = (character_Center.x < block_Center.x);

			//bug 
			if (abs(character_Center.x - block_Center.x) > m_size.x/2.f + block_HalfWidthHeight.x || abs(character_Center.y - block_Center.y) > m_size.y/2.f + block_HalfWidthHeight.y)
			{
				m_CollisionBug = true;
				return;
			}

			if ((IsBelow && m_DeltaMovement.y < 0) || (IsAbove && m_DeltaMovement.y > 0))
			{
				m_position.y -= m_DeltaMovement.y;
				m_speed.y = 0;
			}
			else if ((IsLeft && m_DeltaMovement.x > 0) || (IsRight && m_DeltaMovement.x < 0))
			{
				m_position.x -= m_DeltaMovement.x;
				m_speed.x = 0;
			}

		}
	}

}

void Player::OnContactEnd(b2Fixture * pOther)
{
}


