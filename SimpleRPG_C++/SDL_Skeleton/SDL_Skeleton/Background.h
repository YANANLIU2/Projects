#pragma once
#include <SDL.h>
#include "Defines.h"

class Background
{
public:
	enum EMapIndex
	{
		kOne,

		Num_Types
	};

protected:
	struct Mapdata
	{
		SDL_Texture* m_pTexture;
		const char *m_pfilename;
	};
	SDL_Renderer * m_pRenderer;
	Mapdata m_back;
	Mapdata m_front;
	EMapIndex m_mapIndex;
	SDL_Rect m_src;
	SDL_Rect m_des;

public:
	Background(SDL_Renderer* pRender, EMapIndex mapIndex);
	~Background();
	virtual void Render();
	virtual void Update(Vector2 playerSpeed);
	void LoadATexture();
	void OutofBox();
};

