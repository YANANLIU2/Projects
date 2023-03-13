// TileRenderable.h
#pragma once

struct SDL_Renderer;
struct SDL_Rect;

//---------------------------------------------------------------------------------------------------------------------
// Helper class to handle rendering.
//---------------------------------------------------------------------------------------------------------------------
class TileRenderer
{
    unsigned char m_red, m_green, m_blue, m_alpha;  // tile color

public:
	TileRenderer(unsigned char red, unsigned char green, unsigned char blue, unsigned char alpha)
		: m_red(red)
		, m_green(green)
		, m_blue(blue)
		, m_alpha(alpha) {};
    void Render(SDL_Renderer* pRenderer, const SDL_Rect& rect);
};
