#pragma once
#include "GameState.h"

class KonamiState :
	public GameState
{
	std::vector<const char*> m_creditText = { "KonamiState" ,"Unfinished" , "Unfinished" , "Press P back to Menu" };

public:
	KonamiState(SDL_Renderer* pRenderer);
	~KonamiState();

	void Draw() override;
	bool Update(float deltaSeconds) override;
	void GetInput(const SDL_Event& event) override;
};

