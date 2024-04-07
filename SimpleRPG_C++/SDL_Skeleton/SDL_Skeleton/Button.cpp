#include "Button.h"

Button::Button(SDL_Renderer * pRenderer, SDL_Rect srcRect, SDL_Rect desRect, const char * pSelectedTexturePath, const char * pUnSelectedTexturePath, int id)
	:UIBase(pRenderer, srcRect, desRect, pSelectedTexturePath, pUnSelectedTexturePath, id)

{
	Init();
}

void Button::Init()
{
	m_canSelect = true;
	LoadTextures(m_pRenderer, m_pSelectedTexturePath, m_pUnSelectedTexturePath);
}

Button::~Button()
{
	SDL_DestroyTexture(m_pSelectedTexture);
	SDL_DestroyTexture(m_pUnselectedTexture);
}
