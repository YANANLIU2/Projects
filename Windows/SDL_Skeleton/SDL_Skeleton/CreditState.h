#pragma once
#include "GameState.h"
class CreditState :
	public GameState
{
	std::vector<const char*> m_creditText = { "Button: button pngtree" ,"Font:dafont.com ;Other Graphics: RPG Maker" , "Other Graphics: RPG Maker; Press P back to Menu" , "Press P back to Menu" };
public:
	CreditState(SDL_Renderer* pRenderer);
	~CreditState();
	void Draw() override;
	bool Update(float deltaSeconds) override;
	void GetInput(const SDL_Event& event) override;

};

