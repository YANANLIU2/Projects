#include <iostream>
#include <assert.h>

#include "SDL_ttf.h"
#include "SDL_mixer.h"
#include "tinyxml2.h"

#include "GameStateMachine.h"


using std::cout;

using namespace tinyxml2;

GameStateMachine::GameStateMachine(SDL_Renderer* pRenderer)
{
	m_pRenderer = pRenderer;
	m_isplay = true;
	//size 3: enter, play, end
}

GameStateMachine::~GameStateMachine()
{
	delete m_currentGameState;
	m_currentGameState = nullptr;
}

void GameStateMachine::ChangeState(GameState * newstate)
{
	delete m_currentGameState;
	m_currentGameState = nullptr;

	m_currentGameState = newstate;
}

void GameStateMachine::Update(float deltaSeconds)
{
	//gamestate update report false when it needs to go to the next game state or when exit. 
	if (!m_currentGameState->Update(deltaSeconds))
	{
		if (m_currentGameState->IsExit())
			m_isplay = false;
		else
		{
			ChangeState(m_currentGameState->GetNextGameState());
		}
	}
}

bool GameStateMachine::Iswin()
{
	return m_currentGameState->Iswin();
}

void GameStateMachine::GetInput(const SDL_Event& event)
{

	if (event.type == SDL_KEYDOWN)
	{
		if (event.type == SDL_QUIT)
		{
			m_isplay = false;
		}
		else if (event.type == SDL_KEYDOWN && event.key.keysym.sym == SDLK_ESCAPE)
		{
			m_isplay = false;
		}
	}

	m_currentGameState->GetInput(event);

}

void GameStateMachine::Draw()
{
	m_currentGameState->Draw();
}


