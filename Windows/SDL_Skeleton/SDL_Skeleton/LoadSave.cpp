#include "LoadSave.h"
#include <string>
#include <iostream>
using namespace tinyxml2;
#define Setroot tinyxml2::XMLDocument Doc;Doc.LoadFile(m_filename);tinyxml2::XMLElement* proot = Doc.RootElement();
using std::cout;
//advantages:: clean code
//disadvantage:: if one input child is wrong, it will break

string LoadSave::GetStringInfo(const char *  firstChild, const char *  secondChild, const char *  thirdChild, const char *  fourthChild)
{
	Setroot;
	string m_result = "";
	if (fourthChild)
		m_result = proot->FirstChildElement(firstChild)->FirstChildElement(secondChild)->FirstChildElement(thirdChild)->FirstChildElement(fourthChild)->GetText();
	else if (thirdChild)
		m_result = proot->FirstChildElement(firstChild)->FirstChildElement(secondChild)->FirstChildElement(thirdChild)->GetText();
	else if (secondChild)
		m_result = proot->FirstChildElement(firstChild)->FirstChildElement(secondChild)->GetText();
	else if (firstChild)
		m_result = proot->FirstChildElement(firstChild)->GetText();

	return m_result;
	
}

int LoadSave::GetIntInfo(const char*  firstChild, const char*  secondChild, const char*  thirdChild, const char*  fourthChild)
{
	Setroot;
	int m_result = -1;
	if (fourthChild)
	{
		ReadElementValue(proot->FirstChildElement(firstChild)->FirstChildElement(secondChild)->FirstChildElement(thirdChild), fourthChild, m_result, Int);
	}
	else if (thirdChild)
	{
		ReadElementValue(proot->FirstChildElement(firstChild)->FirstChildElement(secondChild), thirdChild, m_result, Int);
	}
	else if (secondChild)
	{
		ReadElementValue(proot->FirstChildElement(firstChild), secondChild, m_result, Int);
	}
	else if (firstChild)
	{
		ReadElementValue(proot, firstChild, m_result, Int);
	}

	return m_result;
}

//void LoadSave::LoadPlayer(ObjectBase* m_player)
//{
//	Setroot;
//	//load saved values to struct playerInfo
//	ReadElementValue(proot->FirstChildElement("playerInfo"), "health", m_playerInfo.playerHP, Float);
//	//bug: do not have absolute x and y. 
//	//ReadElementValue(proot->FirstChildElement("position"), "x", m_playerInfo.playerPosition.x, Float);
//	//ReadElementValue(proot->FirstChildElement("position"), "y", m_playerInfo.playerPosition.y, Float);
//
//	//check are those values valid?
//	
//	bool Valid = false;
//
//	if (m_playerInfo.playerHP <= 0)
//		Valid = false;
//
//	//load values if valid
//
//	if (Valid)
//	{
//		m_player->Sethp(m_playerInfo.playerHP);
//	}
//}

//void LoadSave::SavePlayer(ObjectBase* m_player)
//{
//	//get value from player ,then save 
//	std::string temp = std::to_string(m_player->GetHp());
//	char const *pchar = temp.c_str();
//	Setroot;
//	proot->FirstChildElement("playerInfo")->FirstChildElement("health")->SetText(pchar);
//}

//void LoadSave::LoadEnterState(EnterState * enterstate)
//{
//	//load background filepath
//	Setroot;
//	enterstate->LoadBackground(proot->FirstChildElement("gamestate")->FirstChildElement("enterstate")->FirstChildElement("background")->GetText());
//}