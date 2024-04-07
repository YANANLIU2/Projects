#include "Background.h"
#include <iostream>
#include <SDL_image.h>
#include "Defines.h"
Background::Background(SDL_Renderer* pRender, EMapIndex mapIndex)
{
	m_src = { 220,2130, Windows_Width/3 - 40, Windows_Height/3-30 };
	m_des = { 0,0, Windows_Width, Windows_Height };

	m_pRenderer = pRender;
	m_mapIndex = mapIndex;
	switch (m_mapIndex)
	{
	case EMapIndex::kOne:
		m_back.m_pfilename = "Assets/Background/C1BG.png";
		m_front.m_pfilename = "Assets/Background/C1FG.png";
		break;
	default:
		std::cout << "Invalid mapIndex.\n";
		break;

	}
	LoadATexture();

}


Background::~Background()
{
}

void Background::Render()
{
	SDL_RenderCopy(m_pRenderer, m_back.m_pTexture, &m_src,&m_des);
	SDL_RenderCopy(m_pRenderer, m_front.m_pTexture, &m_src, &m_des);
	//SDL_RenderCopy(m_pRenderer, m_pTexture, &m_SourRect, &m_DesRect);
}

void Background::Update(Vector2 playerSpeed)
{
	//loss of data
	m_src.x += int(playerSpeed.x);
	m_src.y += int(playerSpeed.y);

	OutofBox();
}

void Background::LoadATexture()
{
	m_back.m_pTexture = IMG_LoadTexture(m_pRenderer, m_back.m_pfilename);
	m_front.m_pTexture = IMG_LoadTexture(m_pRenderer, m_front.m_pfilename);
	if (!m_back.m_pTexture)
		std::cout << "Failed to load back image\n";
	if (!m_front.m_pTexture)
		std::cout << "Failed to load front image\n";
}

void Background::OutofBox()
{
	if (m_src.x > 3000)
		m_src.x = 3000;
	else if (m_src.x < 200)
		m_src.x = 200;

	if (m_src.y > 1800)
		m_src.y = 1800;
	else if (m_src.y < 640)
		m_src.y = 640;
}
