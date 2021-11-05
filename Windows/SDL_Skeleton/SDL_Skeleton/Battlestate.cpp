#include "Battlestate.h"
#include "SoundManager.h"


Battlestate::Battlestate(SDL_Renderer* pRenderer) : GameState(pRenderer)
{

	SoundManager::Get().Play(SoundType::BattleState);
}


Battlestate::~Battlestate()
{
}
