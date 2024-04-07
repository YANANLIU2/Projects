#pragma once

#include "GameState.h"
class EndState : public GameState
{
public:
	EndState(SDL_Renderer* pRenderer);
	~EndState();
};

