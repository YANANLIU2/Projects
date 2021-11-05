#include "UIBase.h"
#include <SDL_image.h>

UIBase::UIBase(SDL_Renderer * pRenderer, SDL_Rect srcRect, SDL_Rect desRect, const char * pSelectedTexturePath, const char *pUnSelectedTexturePath, int id)
	: m_srcRect(srcRect)
	, m_desRect(desRect)
	, m_pRenderer(pRenderer)
	, m_pSelectedTexturePath(pSelectedTexturePath)
	, m_pUnSelectedTexturePath(pUnSelectedTexturePath)
	, m_gameobjectID(id)
{
	m_canSelect = false;
}

UIBase::~UIBase()
{
	if (m_pSelectedTexture)
		SDL_DestroyTexture(m_pSelectedTexture);
	SDL_DestroyTexture(m_pUnselectedTexture);
}

void UIBase::LoadTextures(SDL_Renderer* pRenderer, const char* pSelectedTexturePath, const char* pUnselectedTexturePath)
{
	m_pUnselectedTexture = IMG_LoadTexture(pRenderer, pUnselectedTexturePath);
	if(m_canSelect)
		m_pSelectedTexture = IMG_LoadTexture(pRenderer, pSelectedTexturePath);
}

bool UIBase::IsSelected()
{
	if (!m_canSelect)
		return false;
	return m_isSelected;
}

void UIBase::CheckMousePosition(int x, int y)
{
	SDL_Point p{ x, y };
	bool selectedNow = SDL_PointInRect(&p, &m_desRect);

	if (m_isSelected && !selectedNow)
	{
		m_isSelected = false;
	}
	else if (!m_isSelected && selectedNow)
	{
		m_isSelected = true;
	}
}

void UIBase::Draw()
{
	if(m_canSelect == true)
		SDL_RenderCopy(m_pRenderer, (m_isSelected == true) ? m_pSelectedTexture : m_pUnselectedTexture, &m_srcRect, &m_desRect);
	else
		SDL_RenderCopy(m_pRenderer, m_pUnselectedTexture, &m_srcRect, &m_desRect);
}
