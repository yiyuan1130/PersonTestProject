package.path = CS.UnityEngine.Application.dataPath .. "/Genetic/Example1/Lua/?.lua"

GameObject = CS.UnityEngine.GameObject
Image = CS.UnityEngine.UI.Image
Text = CS.UnityEngine.UI.Text
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
Color = CS.UnityEngine.Color
Button = CS.UnityEngine.UI.Button
Mathf = CS.UnityEngine.Mathf
Quaternion = CS.UnityEngine.Quaternion
RectTransform = CS.UnityEngine.RectTransform
LineRenderer = CS.UnityEngine.LineRenderer
LuaTimer = CS.LuaTimer
TextMesh = CS.UnityEngine.TextMesh

Util = require("Util")
local RandomPoint = require("RandomPoint")
local GA = require("GA")

local pointCount = 8
local points = RandomPoint.SetupPoint(pointCount)
local dna_size = pointCount
local pop_size = 100
local crossover_rate = 0.8
local mutation_rate = 0.1
local generations = 1000000

luatimer = GameObject.Find("LuaTimer"):GetComponent(typeof(LuaTimer))
-- luatimer:Excute(1, function()
--     print("LuaTime Excute")
-- end)
local linerender_cur = GameObject.Find("LineRendererCur"):GetComponent(typeof(LineRenderer))
local linerender_his = GameObject.Find("LineRendererHistory"):GetComponent(typeof(LineRenderer))
local showLine = function(linerender, bestDna)
    linerender.startWidth = 0.1
    linerender.endWidth = 0.1
    linerender.positionCount = #bestDna
    for k, v in ipairs(bestDna) do
        local point = points[v]
        linerender:SetPosition(k - 1, Vector3(point[1], point[2], 0))
    end
end

-- luatimer:Excute(1, function()


GA.init(dna_size, pop_size, crossover_rate, mutation_rate, generations)

local textMesh = GameObject.Find("TextMesh"):GetComponent(typeof(TextMesh))

local history_min_dis = 9999
local history_best_dna = GA.POPULATIONS[1]
for generate = 1, GA.GENERATIONS do
    luatimer:Excute(generate * 0.25, function()
        local point_orders = GA.translateDNA(GA.POPULATIONS, points)
        local fitness = GA.get_fitness(point_orders)
        local str = "代数 : " .. generate .. "  当前种群最优 : " .. fitness[1].dis .. "   历史最优 : " .. history_min_dis
        textMesh.text = str
        print(str)
        -- print("generate:", generate, "cur_min_dis:", fitness[1].dis, "history_min_dis:", history_min_dis--[[, "bestDna:", table.concat(history_best_dna)]])
        if fitness[1].dis < history_min_dis then
            history_min_dis = fitness[1].dis
            history_best_dna = {}
            for k, v in ipairs(GA.POPULATIONS[fitness[1].pop_id]) do
                table.insert(history_best_dna, v)
            end
        end
        showLine(linerender_his, history_best_dna)
        -- showLine(linerender_cur, GA.POPULATIONS[fitness[1].pop_id])
        GA.evolve(fitness)
    end)
end
-- end)