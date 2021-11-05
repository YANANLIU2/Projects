#include "SoundManager.h"

#include <iostream>

SoundManager& SoundManager::Get()
{
	static SoundManager sInstance;
	return sInstance;
}

void SoundManager::Load(const char* filepath, SoundType type, bool isMusic)
{
	Sound loading;
	loading.m_isMusic = isMusic;

	if (isMusic)
	{
		loading.m_data.m_pMusic = Mix_LoadMUS(filepath);
	}
	else
	{
		loading.m_data.m_pChunk = Mix_LoadWAV(filepath);
	}

	auto soundItr = m_soundMap.find(type);
	if (soundItr == m_soundMap.end())
	{
		std::vector<Sound> newSound;
		newSound.push_back(loading);
		m_soundMap.emplace(type, newSound);
	}
	else
	{
		soundItr->second.push_back(loading);
	}
}

void SoundManager::Play(SoundType type)
{
	auto soundItr = m_soundMap.find(type);

	if (soundItr == m_soundMap.end())
	{
		std::cout << "Tried to play a sound that has not been loaded...\n";
		return;
	}

	std::vector<Sound>* pSoundGroup = &soundItr->second;
	Sound* pSound;
	if (pSoundGroup->size() > 1)
	{
		// Same as below with pointer maths!
		//pSound = (pSoundGroup->data() + (rand() % pSoundGroup->size()));
		pSound = &(*pSoundGroup)[rand() % pSoundGroup->size()];
	}
	else
	{
		pSound = &(*pSoundGroup)[0];
	}

	if (pSound->m_isMusic)
	{
		Mix_PlayMusic(pSound->m_data.m_pMusic, -1);
	}
	else
	{
		Mix_PlayChannel(-1, pSound->m_data.m_pChunk, 0);
	}
}

void SoundManager::FreeResources()
{
	for (auto& soundGroup : m_soundMap)
	{
		for (auto& sound : soundGroup.second)
		{
			if (sound.m_isMusic)
			{
				Mix_FreeMusic(sound.m_data.m_pMusic);
			}
			else
			{
				Mix_FreeChunk(sound.m_data.m_pChunk);
			}
		}
	}
}

void SoundManager::LoadAll()
{
	//loading sounds
	//wav
	SoundManager::Get().Load("Assets/Sound/wav/Click.wav", SoundType::Click);
	SoundManager::Get().Load("Assets/Sound/wav/Click_1.wav", SoundType::Click);
	SoundManager::Get().Load("Assets/Sound/wav/Click_2.wav", SoundType::Click);
	SoundManager::Get().Load("Assets/Sound/wav/Click_3.wav", SoundType::Click);
	SoundManager::Get().Load("Assets/Sound/wav/LevelUp.wav", SoundType::LevelUp);
	SoundManager::Get().Load("Assets/Sound/wav/Hit.wav", SoundType::Hit);
	SoundManager::Get().Load("Assets/Sound/wav/Skill.wav", SoundType::Skill);
	SoundManager::Get().Load("Assets/Sound/wav/Walk.wav", SoundType::Walk);
	SoundManager::Get().Load("Assets/Sound/wav/PickACoin.wav", SoundType::PickACoin);
	//music
	SoundManager::Get().Load("Assets/Sound/mp3/Credit.mp3", SoundType::CreditState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Battle.mp3", SoundType::BattleState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Konami.mp3", SoundType::KonamiState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Lose.mp3", SoundType::LoseState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Menu.mp3", SoundType::MenuState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Play.mp3", SoundType::PlayState, true);
	SoundManager::Get().Load("Assets/Sound/mp3/Win.mp3", SoundType::WinState, true);
}

SoundManager::SoundManager()
{
}

// When does a static destructor get called?
SoundManager::~SoundManager()
{
	std::cout << "Destroying Sound Manager...\n";
}