#pragma once

#include <unordered_map>

#include <SDL_mixer.h>

//store sounds
struct Sound
{
	union Data
	{
		Mix_Music* m_pMusic;
		Mix_Chunk* m_pChunk;
	};

	Data m_data;
	bool m_isMusic;
};

//One sound for UI: Click
//five sounds for character: PickACoin, LevelUp, Hit, Walk, Skill
//Seven Music: MenuState, PlayState, WinState, LoseState, BattleState, KonamiState
enum class SoundType
{
	Click,

	PickACoin,
	LevelUp, 
	Hit, 
	Walk, 
	Skill,

	MenuState, 
	PlayState, 
	CreditState,
	WinState, 
	LoseState, 
	BattleState,
	KonamiState,
};

class SoundManager
{
public:
	static SoundManager& Get();

	void Load(const char* filepath, SoundType type, bool isMusic = false);
	void Play(SoundType type);
	void FreeResources();
	void LoadAll();
private:
	SoundManager();
	~SoundManager();
	SoundManager(const SoundManager& copy) = delete;

	std::unordered_map<SoundType, std::vector<Sound>> m_soundMap;
};


