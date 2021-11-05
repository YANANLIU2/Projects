#include <iostream>

#include "CreditState.h"
#include "Font.h"
#include "LoadSave.h"
#include "SoundManager.h"
CreditState::CreditState(SDL_Renderer* pRenderer) :GameState(pRenderer)
{
	//background
	string backgroundpath = LoadSave::Get().GetStringInfo("gamestate", "creditstate", "background", nullptr);
	m_filepath = backgroundpath.c_str();
	LoadATexture(m_filepath);
	//credit
	string port_creditPath = LoadSave::Get().GetStringInfo("ui", "font", "port_credit", nullptr);
	//add to vec
	m_UIObjectVec.push_back(new Font(m_pRenderer, 50, kSDLWhite, m_creditText[0], { 100,200,int(600 * strlen(m_creditText[0]) / 43),50 }, port_creditPath.c_str()));
	m_UIObjectVec.push_back(new Font(m_pRenderer, 50, kSDLWhite, m_creditText[1], { 100,250,int(600 * strlen(m_creditText[1]) / 43),50 }, port_creditPath.c_str()));
	m_UIObjectVec.push_back(new Font(m_pRenderer, 50, kSDLWhite, m_creditText[2], { 100,300,int(600 * strlen(m_creditText[2]) / 43),50 }, port_creditPath.c_str()));
	m_UIObjectVec.push_back(new Font(m_pRenderer, 50, kSDLWhite, m_creditText[3], { 100,350,int(600 *strlen(m_creditText[3]) / 43),50 }, port_creditPath.c_str()));

	//music
	SoundManager::Get().Play(SoundType::CreditState);
}


CreditState::~CreditState()
{
	SDL_DestroyTexture(m_pTexture);
	Clear();
}

void CreditState::Draw()
{
	SDL_RenderCopy(m_pRenderer, m_pTexture, nullptr, nullptr);

	for(auto Obj: m_UIObjectVec)
		Obj->Draw();

}

bool CreditState::Update(float deltaSeconds)
{
	return !m_finish;
}

void CreditState::GetInput(const SDL_Event& event)
{
	if (event.type == SDL_KEYDOWN && event.key.keysym.sym == SDLK_p)
	{
		m_finish = true;
		m_nextState = EGameStates::kMenu;
	}
}
