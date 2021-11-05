#pragma once
#include "GameState.h"
class MenuState: public GameState
{
	enum EButtonMenuState
	{
		kNewGameButton,
		kLoadButton,
		kExitButton,
		kCreditButton,

		Num_Buttons
	};

	bool m_newgame;
	bool m_loadgame;
	bool m_showcredit;
	const char* m_buttonName[4] = { "newGamebutton","loadbutton","creditbutton","exitbutton" };

public:
	MenuState(SDL_Renderer* pRenderer);
	~MenuState();
	void Draw() override;
	bool Update(float deltaSeconds) override;
	void GetInput(const SDL_Event& event) override;
};

