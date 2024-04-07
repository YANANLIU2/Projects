#pragma once
#include "MachineBase.h"
#include "GameState.h"
class PlaystateMachine: public MachineBase
{
	PlaystateMachine(SDL_Renderer* pRenderer);
	~PlaystateMachine();
	PlaystateMachine(const PlaystateMachine& copy) = delete;

public:
	static PlaystateMachine&  Get(SDL_Renderer* pRenderer) { static PlaystateMachine instance(pRenderer); return instance; }
	void Update(float deltaSeconds) override;
	void NextState(GameState::EGameStates newstate) override;
};
