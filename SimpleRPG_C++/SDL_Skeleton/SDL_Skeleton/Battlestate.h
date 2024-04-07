#pragma once

#include "GameState.h"

class Battlestate :
	public GameState
{
public:
	Battlestate(SDL_Renderer* pRenderer);
	~Battlestate();
};

