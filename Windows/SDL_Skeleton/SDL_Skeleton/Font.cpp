#include "Font.h"
#include "SDL_ttf.h"
#include <iostream>
#include "Defines.h"

Font::Font(SDL_Renderer* pRenderer, int fontSize, SDL_Color color, const char * text, SDL_Rect desRect, const char *pFontPath)
	: UIBase(pRenderer, {0,0,0,0}, desRect, "", pFontPath, 0)
{
	// Loading a font
	TTF_Font* m_pTheFont = TTF_OpenFont(pFontPath, fontSize);

	if (!m_pTheFont)
		std::cout << "fail to load font.\n";

	SDL_Surface* m_pTextSurface = TTF_RenderText_Blended(m_pTheFont, text, color);
	m_pUnselectedTexture = SDL_CreateTextureFromSurface(m_pRenderer, m_pTextSurface);
	m_canSelect = false;
}

Font::~Font()
{
	SDL_FreeSurface(m_pTextSurface);
	m_pTextSurface = nullptr;

	//font did not set its selected texture
	SDL_DestroyTexture(m_pUnselectedTexture);
	m_pUnselectedTexture = nullptr;
}

void Font::Draw()
{
	SDL_RenderCopy(m_pRenderer, m_pUnselectedTexture, nullptr, &m_desRect);
}


