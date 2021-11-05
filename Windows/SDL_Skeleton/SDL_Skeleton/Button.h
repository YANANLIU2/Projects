#pragma once
#include "UIBase.h"
class Button :
	public UIBase
{

public:
	Button(SDL_Renderer* pRenderer, SDL_Rect srcRect, SDL_Rect desRect, const char * pSelectedTexturePath, const char *pUnSelectedTexturePath, int id);
	void Init() override;
	~Button();
};

