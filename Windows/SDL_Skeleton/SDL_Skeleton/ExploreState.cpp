#include <iostream>
#include "ExploreState.h"
#include "WoodsMap.h"
#include "CharacterBase.h"

ExploreState::ExploreState(SDL_Renderer* pRenderer, CharacterBase* player, b2World* pWorld): GameState(pRenderer)
{
	m_pWorld = pWorld;
	m_pPlayer = player;
	this->SetMap(Map::EMapType::kWoods);
}

void ExploreState::Draw()
{
	m_pCurrentMap->Draw(m_pPlayer);

	//box2d debug draw
	m_pWorld->DrawDebugData();
	SDL_RenderPresent(m_pRenderer);
}


ExploreState::~ExploreState()
{
	delete m_pCurrentMap;
	m_pCurrentMap = nullptr;
}

void ExploreState::SetMap(Map::EMapType maptype)
{
	if (m_pCurrentMap)
	{
		delete m_pCurrentMap;
		m_pCurrentMap = nullptr;
	}

	switch (maptype)
	{
	case Map::EMapType::kWoods:
		m_pCurrentMap = new WoodsMap(m_pRenderer,m_pWorld);
		break;
	default:
		std::cout << "Invalid Map.\n";
		break;
	}
}

void ExploreState::GetInput(const SDL_Event & event)
{

	if (event.type == SDL_KEYDOWN)
	{
		KonamiCheck(event);

		if (event.key.keysym.sym == SDLK_d || event.key.keysym.sym == SDLK_RIGHT)
		{
			m_pPlayer->MoveDir(EMove::Right);
		}
		if (event.key.keysym.sym == SDLK_a || event.key.keysym.sym == SDLK_LEFT)
		{
			m_pPlayer->MoveDir(EMove::Left);
		}
		if (event.key.keysym.sym == SDLK_w || event.key.keysym.sym == SDLK_UP)
		{
			m_pPlayer->MoveDir(EMove::Up);
		}
		if (event.key.keysym.sym == SDLK_s || event.key.keysym.sym == SDLK_DOWN)
		{
			m_pPlayer->MoveDir(EMove::Down);
		}
	}

	else if (event.type == SDL_KEYUP)
	{
		if (event.key.keysym.sym == SDLK_d || event.key.keysym.sym == SDLK_RIGHT)
		{
			m_pPlayer->MoveDir(EMove::StopRight);
		}
		if (event.key.keysym.sym == SDLK_a || event.key.keysym.sym == SDLK_LEFT)
		{
			m_pPlayer->MoveDir(EMove::StopLeft);
		}
		if (event.key.keysym.sym == SDLK_w || event.key.keysym.sym == SDLK_UP)
		{
			m_pPlayer->MoveDir(EMove::StopUp);
		}
		if (event.key.keysym.sym == SDLK_s || event.key.keysym.sym == SDLK_DOWN)
		{
			m_pPlayer->MoveDir(EMove::StopDown);
		}
	}
}
