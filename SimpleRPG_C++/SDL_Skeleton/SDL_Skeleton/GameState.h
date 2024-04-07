#pragma once
#ifndef  GAMESTATE_H
#define GAMESTATE_H

#include <SDL_events.h>
#include "SDL_render.h"
#include "Defines.h"
#include <vector>

class UIBase;
class CharacterBase;
class LoadSave;

class GameState
{
public:
	enum EGameStates
	{
		kMenu,
		kPlay,
		kEnd,
		kCredit,
		kBattle,
		kKonami,
		kExplore,

		Num_GameStates
	};

protected:
	SDL_Texture * m_pTexture;
	SDL_Renderer * m_pRenderer;
	const char* m_filepath;

	bool m_exit;
	bool m_finish;
	std::vector<UIBase*> m_UIObjectVec;
	EGameStates m_nextState;

	//use vector; in case I want to customize the code in game later. 
	std::vector <int> m_KonamiCode = { SDLK_UP,SDLK_UP,SDLK_DOWN, SDLK_DOWN, SDLK_LEFT, SDLK_RIGHT,SDLK_LEFT, SDLK_RIGHT, SDLK_b, SDLK_a, SDLK_RETURN };
	int m_currentkonamiIndex;

public:
	GameState(SDL_Renderer* pRenderer) { m_pRenderer = pRenderer; m_exit = false; m_finish = false; m_currentkonamiIndex = 0;}
	virtual ~GameState();

	//return true to keep updating, retrun false to exit or go to next game state
	virtual bool Update(float deltaSeconds) { return false; }
	virtual void Draw() {};
	virtual bool Iswin() { return false; }
	virtual void Collision() {};
	virtual void GetInput(const SDL_Event& event) {};
	void LoadBackground(const char* filename) { m_filepath = filename; }
	void CheckMousePosition(int x, int y);
	GameState* GetNextGameState();
	bool IsExit() { return m_exit; }
	void Clear();
	void LoadATexture(const char * filepath);

	//konami check
	void KonamiCheck(const SDL_Event& event);

};

#endif // ! GAMESTATE_H
