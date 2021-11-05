#pragma once
#ifndef TILE_H
#define TILE_H
#include "Defines.h"
#include "SDL.h"
#include <SDL_image.h>
#include "Box2D.h"

class CharacterBase;
class Tile
{
public:
	static SDL_Texture* m_pWoodSetTexture;
	static SDL_Texture* m_pWoodSetTextureTransparent;

protected:
	SDL_Texture * m_pTexture;
	SDL_Renderer * m_pRenderer;
	const char* m_filepath;

private:

	ETileType m_tileType;

	SDL_Rect m_srcRect;
	SDL_Rect m_desRect;
	bool m_isBlock;

	b2Body* m_pBody;
	b2Fixture* m_pFixture;

public:
	Tile(SDL_Renderer* pRenderer, ETileType tiletype, SDL_Texture* theTexture, SDL_Rect srcRect, SDL_Rect desRect);
	void Init() {};
	virtual ~Tile();
	bool IsBlocked() { return m_isBlock; }
	virtual void Draw();
	void SetTextures(SDL_Texture* texture);
	ETileType GetTileType() { return m_tileType; }
	SDL_Rect GetDesRect() { return m_desRect; }
	static SDL_Texture * GetTexture(ETileType type, bool tranparent);
	void SetDesRect(SDL_Rect desRect) { m_desRect = desRect; }
	static void TileTextureSetup(SDL_Renderer* pRenderer);
};

#endif // !TILE_H
