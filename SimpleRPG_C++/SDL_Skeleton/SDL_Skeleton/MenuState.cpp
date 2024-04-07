#include <iostream>

#include "MenuState.h"
#include "LoadSave.h"
#include "Button.h"
#include "Font.h"
#include "SoundManager.h"

MenuState::MenuState(SDL_Renderer* pRenderer):GameState(pRenderer)
{
	m_newgame = m_loadgame = m_showcredit = false;
	//load font path
	string TrueliesPath = LoadSave::Get().GetStringInfo("ui", "font", "trueLies", nullptr);
	string d_day_stencilPath = LoadSave::Get().GetStringInfo("ui", "font", "d_day_stencil", nullptr);
	//load backgroundpath
	string backgroundpath = LoadSave::Get().GetStringInfo("gamestate", "menustate", "background", nullptr);
	m_filepath = backgroundpath.c_str();
	LoadATexture(m_filepath);

	//create button and corresponding font; 
	SDL_Rect ButtonDesRect;
	ButtonDesRect.h = 46;
	ButtonDesRect.y = (WINDOW_HEIGHT >> 1) - (ButtonDesRect.h >> 1);
	ButtonDesRect.w = int(152*1.5);
	ButtonDesRect.x = (WINDOW_WIDTH >> 1) - (ButtonDesRect.w >> 1);

	string TheButtonSelectedPath = LoadSave::Get().GetStringInfo("ui", "button", "blueMedium", "selected");
	string TheButtonUnselectedPath = LoadSave::Get().GetStringInfo("ui", "button", "blueMedium", "unselected");

	SDL_Rect ButtonSrcRect;
	ButtonSrcRect.y = ButtonSrcRect.x = 0;
	ButtonSrcRect.h = LoadSave::Get().GetIntInfo("ui", "button", "blueMedium", "height");
	ButtonSrcRect.w = LoadSave::Get().GetIntInfo("ui", "button", "blueMedium", "width");

	//get the initial value; add button and font to vec, button first, then its corresponding font (draw order);
	float standardTextlen = 5.0;
	for (int i = 0; i < EButtonMenuState::Num_Buttons; ++i)
	{
		UIBase* theButton = new Button(pRenderer, ButtonSrcRect, ButtonDesRect, TheButtonSelectedPath.c_str(), TheButtonUnselectedPath.c_str(), LoadSave::Get().GetIntInfo(m_buttonName[i], "id", nullptr, nullptr));
		m_UIObjectVec.push_back(theButton);

		string theFontText = LoadSave::Get().GetStringInfo(m_buttonName[i], "text", nullptr, nullptr);
		float theTextRatio = float(strlen(theFontText.c_str())) / standardTextlen;
		SDL_Rect theFontRect = { ButtonDesRect.x + int(152 * 1.5/2) - int(89 * theTextRatio) /2, ButtonDesRect.y + (46/2) - 32/2, int(89* theTextRatio), 32 };
		UIBase* theFont = new Font(pRenderer, 60, kSDLWhite, theFontText.c_str(), theFontRect, d_day_stencilPath.c_str());
		m_UIObjectVec.push_back(theFont);

		ButtonDesRect.y += int(ButtonDesRect.h*1.5);
	}
	
	//create title font 
	SDL_Rect titleRect;
	titleRect.h = 100;
	titleRect.y = (WINDOW_HEIGHT >> 2) - (titleRect.h >> 1);
	titleRect.w = 400;
	titleRect.x = (WINDOW_WIDTH >> 1) - (titleRect.w >> 1);

	m_UIObjectVec.push_back(new Font (m_pRenderer,99, kSDLWhite,GameName, titleRect, TrueliesPath.c_str()));

	//music
	SoundManager::Get().Play(SoundType::MenuState);

}

MenuState::~MenuState()
{
	Clear();
}

void MenuState::Draw()
{
	//background
	SDL_RenderCopy(m_pRenderer,m_pTexture, nullptr, nullptr);
	//gameObjects
	for (auto Obj : m_UIObjectVec)
	{
		Obj->Draw();
	}
}

bool MenuState::Update(float deltaSeconds)
{
	if (m_exit||m_newgame||m_loadgame||m_showcredit)
		return false;
	return true;
}

void MenuState::GetInput(const SDL_Event& event)
{
	if (event.type == SDL_MOUSEMOTION)
	{
		this->CheckMousePosition(event.motion.x, event.motion.y);
	}

	if (event.type == SDL_MOUSEBUTTONDOWN)
	{
		//left click
		if (event.button.button == SDL_BUTTON_LEFT)
		{
			int buttonId = -1;

			for (auto Obj : m_UIObjectVec)
			{
				if (Obj->IsSelected())
				{
					buttonId = Obj->GetId();
				}
			}
		
			switch (buttonId)
			{
			case 10001:
				m_newgame = true;
				m_nextState = EGameStates::kPlay;
				std::cout << "click newgame\n";
				break;

			case 10002:
				m_loadgame = true;
				m_nextState = EGameStates::kPlay;
				std::cout << "click loadgame\n";
				break;

			case 10003:
				m_showcredit = true;
				m_nextState = EGameStates::kCredit;
				std::cout << "click showcredit\n";
				break;

			case 10004:
				m_exit = true;
				std::cout << "click exit\n";
				break;

			default:
				std::cout << "Invalid Input\n";
				break;
			}

			SoundManager::Get().Play(SoundType::Click);
			return;
		}
	}

}

