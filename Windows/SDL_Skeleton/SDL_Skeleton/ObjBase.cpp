#include "ObjBase.h"
#include <SDL_image.h>
#include <iostream>
using std::cout;

//ObjBase::ObjBase(int gameObjectID):m_gameObjectID(gameObjectID)
//{
//}

ObjBase::ObjBase() 
{
}

ObjBase::~ObjBase()
{
}

void ObjBase::LoadATexture(const char * filepath)
{
	m_filepath = filepath;
	if (m_pRenderer)
	{
		if (m_pTexture)
		{
			delete m_pTexture;
			m_pTexture = nullptr;
		}
		m_pTexture = IMG_LoadTexture(m_pRenderer, filepath);

	}
	if (!m_pTexture)
		cout << "Failed to load Object \n";
}

