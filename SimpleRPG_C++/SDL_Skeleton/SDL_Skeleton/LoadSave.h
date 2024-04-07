#pragma once
#include "Defines.h"
#include "tinyxml2.h"
#include <string>
using std::string;


class MenuState;
class CharacterBase;
class LoadSave
{
	static LoadSave* s_instance;
	const char* m_filename;
	LoadSave() { m_filename = "Data\\GameInfo.xml"; }
	~LoadSave() {};

public:
	static LoadSave& Get() {static LoadSave instance; return instance; }
	string GetStringInfo(const char*  firstChild, const char*  secondChild, const char*  thirdChild, const char*  fourthChild);
	int GetIntInfo(const char*  firstChild, const char*  secondChild, const char*  thirdChild, const char*  fourthChild);
};
