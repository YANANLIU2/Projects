#pragma once
#include <SDL_render.h>
#include <vector>

class ObjBase
{
protected:
	SDL_Texture * m_pTexture;
	SDL_Renderer * m_pRenderer;
	const char* m_filepath;
	//int m_gameObjectID;
public:
	//ObjBase(int gameObjectID);
	ObjBase();
	~ObjBase();
	void LoadATexture(const char * filepath);
};

