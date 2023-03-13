// TileRenderer.cpp
#include "TileRenderer.h"
#include "SDL.h"

//---------------------------------------------------------------------------------------------------------------------
// Renders the tile.
//      -pRenderer: The SDL renderer to use.
//      -rect:      The SDL rect describing the size and position of the tile.
//---------------------------------------------------------------------------------------------------------------------
void TileRenderer::Render(SDL_Renderer* pRenderer, const SDL_Rect& rect)
{
    SDL_SetRenderDrawColor(pRenderer, m_red, m_green, m_blue, m_alpha);
    SDL_RenderFillRect(pRenderer, &rect);
}

