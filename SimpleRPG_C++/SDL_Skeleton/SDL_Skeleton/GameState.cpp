#include <iostream>

#include <SDL_image.h>

#include "GameState.h"
#include "CharacterBase.h"
#include "UIBase.h"

#include "Battlestate.h"
#include "ExploreState.h"
#include "CreditState.h"
#include "EndState.h"
#include "KonamiState.h"
#include "MenuState.h"
#include "PlayState.h"

GameState::~GameState()
{
	Clear();
}

void GameState::CheckMousePosition(int x, int y)
{
	for (auto Obj : m_UIObjectVec)
		Obj->CheckMousePosition(x, y);
}

GameState * GameState::GetNextGameState()
{
	switch (m_nextState)
	{
	case EGameStates::kBattle:
		return new Battlestate(m_pRenderer);
	case EGameStates::kCredit:
		return new CreditState(m_pRenderer);
	case EGameStates::kEnd:
		return new EndState(m_pRenderer);
	case EGameStates::kKonami:
		return new KonamiState(m_pRenderer);
	case EGameStates::kMenu:
		return new MenuState(m_pRenderer);
	case EGameStates::kPlay:
		return new PlayState(m_pRenderer);
	default:
		std::cout << "Invalid State\n";
		return nullptr;
	}
}

void GameState::Clear()
{
	SDL_DestroyTexture(m_pTexture);


	for (auto Obj : m_UIObjectVec)
	{
		delete Obj;
		Obj = nullptr;
	}

	m_UIObjectVec.clear();
	
}

void GameState::LoadATexture(const char * filepath)
{
	m_filepath = filepath;
	if (m_pRenderer)
	{
		if (m_pTexture)
		{
			SDL_DestroyTexture(m_pTexture);
			m_pTexture = nullptr;
		}
		m_pTexture = IMG_LoadTexture(m_pRenderer, filepath);

	}
	if (!m_pTexture)
		std::cout << "Failed to load Object \n";
}

void GameState::KonamiCheck(const SDL_Event & event)
{
	if (event.key.keysym.sym == m_KonamiCode[m_currentkonamiIndex])
	{
		m_currentkonamiIndex++;
		if (m_currentkonamiIndex == m_KonamiCode.size())
		{
			m_currentkonamiIndex = 0;
			m_nextState = EGameStates::kKonami;
			m_finish = true;
		}
	}
	else
	{
		m_currentkonamiIndex = 0;
	}
}
