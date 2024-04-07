#pragma once
#include "UIBase.h"
constexpr SDL_Color kSDLWhite{ 255, 255, 255, 255 };

class Font :
	public UIBase
{
	SDL_Surface* m_pTextSurface;
public:
	Font(SDL_Renderer* pRenderer,int fontSize, SDL_Color color, const char * text, SDL_Rect desRect,const char *pUnSelectedTexturePath);
	~Font();
	void Draw() override;
};

