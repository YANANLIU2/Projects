#include "Tile.h"
#include <iostream>
using std::cout;
#include <string>
#include "SDL_image.h"
#include "CharacterBase.h"

SDL_Texture* Tile::m_pWoodSetTexture = nullptr;
SDL_Texture* Tile::m_pWoodSetTextureTransparent = nullptr;

Tile::Tile(SDL_Renderer * pRenderer, ETileType tiletype, SDL_Texture* theTexture, SDL_Rect srcRect, SDL_Rect desRect)
	: m_tileType(tiletype)
	, m_srcRect(srcRect)
	, m_desRect(desRect)
{
	m_pRenderer = pRenderer;
	m_pTexture = theTexture;
	m_tileType = tiletype;
}

Tile::~Tile()
{
}

void Tile::Draw()
{
	SDL_RenderCopy(m_pRenderer, m_pTexture, &m_srcRect, &m_desRect);
}

void Tile::SetTextures(SDL_Texture* texture)
{
	m_pTexture = texture;
}

SDL_Texture * Tile::GetTexture(ETileType type, bool tranparent)
{
	switch (type)
	{
	case kFloor:
	case kTree:
	case kYellowFlower:
	case kOrangeMushroom:
	{
		if (tranparent)
			return m_pWoodSetTextureTransparent;
		else
			return m_pWoodSetTexture;
	}
	default:
		return nullptr;
	}

}

void Tile::TileTextureSetup(SDL_Renderer* pRenderer)
{
	if (pRenderer)
	{
		m_pWoodSetTexture = IMG_LoadTexture(pRenderer, "Assets/Tile/002-Woods01.png");
		m_pWoodSetTextureTransparent = IMG_LoadTexture(pRenderer, "Assets/Tile/002-Woods01.png");
		SDL_SetTextureAlphaMod(m_pWoodSetTextureTransparent, 150);
	}

	if (!m_pWoodSetTexture)
		cout << "Failed to load Object \n";
}






