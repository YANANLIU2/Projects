#pragma once
#include "CharacterBase.h"
#include "Defines.h"
#include <iostream>
#include "UIBase.h"
using std::cout;

//for template class. the functions needed to be inline or in header file, or it wont compile

template<class Type>
struct Node
{
	Node<Type>* m_pNext;
	Node<Type>* m_pPrev;
	Type* m_object;  // data
	int m_GameObjectID;

	Node<Type>(Type* object, int GameObjectID) : m_pNext(nullptr), m_pPrev(nullptr), m_object(object), m_GameObjectID(GameObjectID) { }
	Node<Type>(Type* object) : m_pNext(nullptr), m_pPrev(nullptr), m_object(object) { }
	int GetObjectId() { return m_GameObjectID; }
	~Node<Type>() { delete m_pNext; }
	Type* GetObject() { return m_object; }
	void ClearPointers() { delete m_object; m_object = nullptr; m_pNext = m_pPrev = nullptr; }
	bool IsSelected() { return m_object->IsSelected(); }
};

template<class Type>
class LinkedList
{
public:

	//enum EGameObjectId
	//{
	//	kPlayer = 20001;
	//	kenmey = 21001;
	//};

	Node<Type>* m_pHead;

public:
	LinkedList();
	~LinkedList();

	void InsertFront(Type* object, int GameObjectID);  // O(1)
	void DeleteCurrentNode();  // O(n)
	void DeleteAll();
	void UpdateAll(float deltaSeconds, Vector2 playerPosition);
	void InitAll();
	void DrawAll();
	void DrawTileMap(int mapwidth, int mapheight);
	void CheckMousePositionAll(int x, int y);
	Node<Type>* FindNode(int objectId) const;

private:
};

template<class Type>
LinkedList<Type>::LinkedList()
	: m_pHead(nullptr)
{
}

template<class Type>
LinkedList<Type>::~LinkedList()
{
	delete m_pHead;
}

template<class Type>
void LinkedList<Type>::InsertFront(Type* object, int GameObjectID)
{
	Node<Type>* pNode = new Node<Type>(object, GameObjectID);
	pNode->m_pNext = m_pHead;
	if (m_pHead)
		m_pHead->m_pPrev = pNode;
	m_pHead = pNode;
}

template<class Type>
void LinkedList<Type>::DeleteCurrentNode()
{

}

template<class Type>
void LinkedList<Type>::DeleteAll()
{
	Node<Type>* pCurrent = m_pHead;

	while (pCurrent->m_pPrev)
	{
		if (pCurrent->m_pPrev)
			pCurrent->m_pPrev->m_pNext = pCurrent->m_pNext;
		else
			m_pHead = pCurrent->m_pNext;
		if (pCurrent->m_pNext)
			pCurrent->m_pNext->m_pPrev = pCurrent->m_pPrev;

		pCurrent->ClearPointers();
		delete pCurrent;
	}
}

template<class Type>
void LinkedList<Type>::UpdateAll(float deltaSeconds, Vector2 playerPosition)
{
	if (!m_pHead)
		return;
	Node<Type>* pCurrent = m_pHead;
	while (pCurrent)
	{
		if (pCurrent->GetObjectId() == 20001)
			pCurrent->GetObject()->Update(deltaSeconds);
		else if (pCurrent->GetObjectId() == 21001)
			pCurrent->GetObject()->Update(deltaSeconds, playerPosition);

		pCurrent = pCurrent->m_pNext;
	}
}

template<class Type>
void LinkedList<Type>::InitAll()
{
	if (!m_pHead)
		return;
	Node<Type>* pCurrent = m_pHead;
	while (pCurrent)
	{
		pCurrent->GetObject()->Init();
		pCurrent = pCurrent->m_pNext;
	}
}

template<class Type>
void LinkedList<Type>::DrawAll()
{
	if (!m_pHead)
		return;
	Node<Type>* pCurrent = m_pHead;
	while (pCurrent)
	{
		pCurrent->GetObject()->Draw();
		pCurrent = pCurrent->m_pNext;
	}
}

template<class Type>
inline void LinkedList<Type>::CheckMousePositionAll(int x, int y)
{
	
	if (!m_pHead)
		return;
	Node<Type>* pCurrent = m_pHead;
	while (pCurrent)
	{
		//only UIBase obj has CheckMousePosition()
		pCurrent->GetObject()->CheckMousePosition(x, y);
		pCurrent = pCurrent->m_pNext;
	}
}

//return the node with the id. return null if there is no such a node. 
template<class Type>
Node<Type>* LinkedList<Type>::FindNode(int objectId) const
{
	if (!m_pHead)
		return nullptr;
	Node<Type>* pCurrent = m_pHead;
	while (pCurrent)
	{
		if (pCurrent->GetObjectId() == objectId)
		{
			return pCurrent;
		}
		else
			pCurrent = pCurrent->m_pNext;
	}
	return nullptr;
}

