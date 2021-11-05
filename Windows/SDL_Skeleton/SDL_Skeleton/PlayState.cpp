#include <iostream>

#include "PlayState.h"
#include "Player.h"
#include "Enemy.h"
#include "LoadSave.h"
#include "SoundManager.h"
#include "GameStateMachine.h"
#include "ExploreState.h"
#include "CollisionHandler.h"

PlayState::PlayState(SDL_Renderer* pRenderer):GameState(pRenderer)
{
	//Box2D setup
	//box2d World
	m_pWorld = new b2World(b2Vec2(0, 7.5f));
	m_pDebugDraw.Init(pRenderer);
	m_pDebugDraw.SetFlags(b2Draw::e_shapeBit);
	m_pWorld->SetDebugDraw(&m_pDebugDraw);
	m_pWorld->SetContactListener(&m_TheCollisionHandler); // enable collision listener

	//create player
	m_pPlayer = new Player(pRenderer, CharacterBase::ECharacterImageType::kFighter01, m_pWorld);

	//Init player
	m_pPlayer->Init();

	//Play music
	SoundManager::Get().Play(SoundType::PlayState);

	//set Ingame State machine, create new explore state and store it as a variable. 
	m_IngameStateMachine = new GameStateMachine(m_pRenderer);
	m_explorState = new ExploreState(m_pRenderer, m_pPlayer,m_pWorld);
	m_IngameStateMachine->ChangeState(m_explorState);
}


PlayState::~PlayState()
{
	delete m_IngameStateMachine;
	m_IngameStateMachine = nullptr;

	delete m_pPlayer;
	m_pPlayer = nullptr;

	SDL_DestroyTexture(m_pTexture);
	delete m_pWorld;
	Clear();
}

void PlayState::Draw()
{
	m_IngameStateMachine->Draw();

}

bool PlayState::Update(float deltaSeconds)
{

	if (m_finish || m_exit)
		return false;
	else
	{
		m_pWorld->Step(deltaSeconds, 8, 3);

		m_IngameStateMachine->Update(deltaSeconds);
		m_pPlayer->Update(deltaSeconds);

		if (m_pPlayer->GetCollisionBug())
		{
			m_TheCollisionHandler.ClearContacts();
			m_pPlayer->SetCollisionBug(false);
			m_pPlayer->SetCollisionBody();
		}

		m_TheCollisionHandler.HandleCollisions();
		return true;
	}
}

void PlayState::GetInput(const SDL_Event& event)
{
	m_IngameStateMachine->GetInput(event);
}


