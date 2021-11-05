#pragma once
#include <SDL_events.h>
#include "SDL_render.h"

#include "Defines.h"
#include "GameState.h"
class LoadSave;
class GameStateMachine
{
private:
	SDL_Renderer* m_pRenderer;
	bool m_isplay;
	GameState* m_currentGameState;

	bool m_win;

public:
	GameStateMachine(SDL_Renderer* pRenderer);
	~GameStateMachine();

	void Update(float deltaSeconds);
	void ChangeState(GameState* newstate);
	bool Iswin();
	void setWin(bool win) { m_win = win; }
	void GetInput(const SDL_Event& event);
	void Draw();
	bool IsPlay() { return m_isplay; }
};

