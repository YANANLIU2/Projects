#include "EndState.h"

EndState::EndState(SDL_Renderer* pRenderer):GameState(pRenderer)
{
	//win and lose differnet music
	//SoundManager::Get().Play(SoundType::

}


EndState::~EndState()
{
	SDL_DestroyTexture(m_pTexture);
	Clear();
}
