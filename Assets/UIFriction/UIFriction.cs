using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriction : MonoBehaviour {
	public float vRatio = 100f;
	public float fRatio = 50 * 9.8f;
	RectTransform bottle;
	void Start () {
		bottle = GameObject.Find("UIRoot").transform.Find("bottle").GetComponent<RectTransform>();
	}
	
	float dragDis = 0f;
	float dragTime = 0f;
	bool canBottleMove = false;
	float bottleSpeed = 0f;
	float startSpeed = 0f;
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)){
			dragTime = Time.fixedTime;
			dragDis = Input.mousePosition.x;
		}
		if (Input.GetMouseButtonUp(0)){
			dragDis = Input.mousePosition.x - dragDis;
			dragTime = Time.fixedTime - dragTime;

			if (dragDis <= 0){
				canBottleMove = false;
				bottleSpeed = 0f;
				return;
			}

			canBottleMove = true;

			float drageScale = dragDis / Screen.width;
			startSpeed = drageScale / dragTime * vRatio;
			bottleSpeed = startSpeed;
		}

		if (canBottleMove){
			BottleMove();
		}
	}

	void BottleMove(){
		if (bottleSpeed <= 0){
			canBottleMove = false;
			return;
		}
		// if (bottle.anchoredPosition.x > xxx){
		// 	// 超出桌面
		// 	canBottleMove = false;
		// 	// todo:掉下的逻辑
		// }

		float a = GetAccelerated();
		bottleSpeed = bottleSpeed - a * Time.fixedDeltaTime;
		bottle.transform.position = bottle.transform.position + new Vector3(bottleSpeed * Time.fixedDeltaTime, 0, 0);
	}

	float GetAccelerated(){
		float bottlePosX = bottle.anchoredPosition.x;
		float f = 0.2f;
		if (bottlePosX > 200 && bottlePosX <= 700){
			f = 0.5f;
		}
		else if (bottlePosX > 700 && bottlePosX <= 1200){
			f = - 0.5f;
		}
		else if (bottlePosX > 1200 && bottlePosX < 1700){
			f = 0.8f;
		}
		return f * fRatio;
	}

	void OnGUI(){
		if (GUILayout.Button("ReStart")){
			bottle.anchoredPosition = Vector3.zero;
		}
	}

	/*
	local bottleStartSpeed = 1000
	local previrousSpeed = bottleStartSpeed
	function update() 
		currentSpeed = previrousSpeed - getF() * Time.fixedDeltaTime
		previrousSpeed = currentSpeed

		bottle.transform.position = bottle.transform.position + Vector3(currentSpeed * Time.fixedDeltaTime, 0, 0)
	end

	function getF()
		-- 默认 -> 0.3
		-- 500 - 690 -> -0.2 水
		-- 1000 - 1500 -> 0.8  沙子

		local f = 0.3
		local bottleX = bottle.transform.position.x
		if bottleX > 500 and bottleX < 690 then
			f = -0.2
		elseif bottleX > 1000 and bottleX < 1500 then
			f = 0.8
		end
		return f
	end


	local area = {
		[1] = {
			id = 2,
			hid = 2, -- image_pos, image_width, f
		},
		[2] = {
			id = 3,
			hid = 3,
		},
	}

	-- 服务器转为下面数据
	local areaTable = {
		[1] = { -- 沙子
			left = image_pos - image_width / 2,		 -- 100
			right = image_pos + image_width / 2,	 -- 200
			f = _f,
		},
		[2] = { -- 水
			left = image_pos - image_width / 2,		-- 10
			right = image_pos + image_width / 2,	-- 50
			f = _f,
		},
	}

	function getF()
		local f = 0.3
		local bottleX = bottle.transform.position.x
		for i = 1, #areaTable do
			local t = areaTable[i]
			if bottleX > left and bottleX < right then
				return t.f
			end
		end
		return f
	end
	*/
}
