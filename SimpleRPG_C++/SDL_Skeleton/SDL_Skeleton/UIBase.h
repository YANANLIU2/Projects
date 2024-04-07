#pragma once
#include "SDL.h"
#ifndef UIBASE_H
#define UIBASE_H

class UIBase
{
protected:
	SDL_Rect m_srcRect;
	SDL_Rect m_desRect;
	const char *m_pSelectedTexturePath;
	const char *m_pUnSelectedTexturePath;

	bool m_canSelect;

	SDL_Texture*	m_pSelectedTexture;
	SDL_Texture*	m_pUnselectedTexture;

	SDL_Renderer * m_pRenderer;
	bool			m_isSelected;
	int m_gameobjectID;

public:
	UIBase(SDL_Renderer* pRenderer, SDL_Rect srcRect, SDL_Rect desRect, const char * pSelectedTexturePath, const char *pUnSelectedTexturePath, int id);
	virtual ~UIBase();
	virtual void Draw();
	void CheckMousePosition(int x, int y);
	void LoadTextures(SDL_Renderer* pRenderer, const char* pSelectedTexturePath, const char* pUnselectedTexturePath);
	virtual void Init() {};
	bool IsSelected();
	int GetId() { return m_gameobjectID; }
};

#endif // !UIBASE_H
