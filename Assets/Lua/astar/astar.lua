local GameObject = CS.UnityEngine.GameObject
local Image = CS.UnityEngine.UI.Image
local Text = CS.UnityEngine.UI.Text
local Vector3 = CS.UnityEngine.Vector3
local Vector2 = CS.UnityEngine.Vector2
local Color = CS.UnityEngine.Color

local AStar = {}

function AStar:init()
    self.canvas = GameObject.Find("Canvas")
    self.ground = self.canvas.transform:Find("ground")
    self.point = self.ground:Find("point")
    self.point.gameObject:SetActive(false)
    self:create_map()
end

function AStar:create_map()
    -- 27 * 15
    self.startPoint = {9, 5}
    self.endPoint = {23, 13}
    local wall_pos = {
        row = {
            pos = 8,
            min = 6,
            max = 22,
        },
        col = {
            pos = 14,
            min = 4,
            max = 12,
        }
    }
    self.points = {}
    self.wall = {}
    for i = 1, 405 do
        local pointObj = GameObject.Instantiate(self.point)
        pointObj.gameObject:SetActive(true)
        pointObj.transform:SetParent(self.ground)
        pointObj.transform.localScale = Vector3.one
        pointObj.transform.localPosition = Vector3.one
        local img = pointObj:GetComponent(typeof(Image))
        local x = math.fmod(i - 1, 27) + 1
        local y = math.modf((i - 1) / 27) + 1
        local name = x .. "-" .. y
        pointObj.name = name
        local tex_F = pointObj.transform:Find("F"):GetComponent(typeof(Text))
        local tex_G = pointObj.transform:Find("G"):GetComponent(typeof(Text))
        local tex_H = pointObj.transform:Find("H"):GetComponent(typeof(Text))
        local tex_pos = pointObj.transform:Find("pos"):GetComponent(typeof(Text))
        tex_pos.text = string.format("%d,%d", x, y)
        tex_G.text = tostring(x)
        tex_H.text = tostring(y)

        local point = {
            name = name,
            pos = {x, y},
            G = 0,
            H = 0,
            F = 0,
            tex_F = tex_F,
            tex_H = tex_H,
            tex_G = tex_G,
            img = img,
        }

        if y == wall_pos.row.pos and x >= wall_pos.row.min and x <= wall_pos.row.max or 
           x == wall_pos.col.pos and y >= wall_pos.col.min and y <= wall_pos.col.max then
            table.insert(self.wall, point)
        else
            self.points[name] = point
            -- table.insert(self.points, point)
        end
    end

    for k,v in pairs(self.wall) do
        v.img.color = Color.black
    end


    self.points[self.startPoint[1] .. '-' .. self.startPoint[2]].img.color = Color.green
    self.points[self.endPoint[1] .. '-' .. self.endPoint[2]].img.color = Color.red

end

AStar:init()
