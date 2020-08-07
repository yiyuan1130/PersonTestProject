local GameObject = CS.UnityEngine.GameObject
local Image = CS.UnityEngine.UI.Image
local Text = CS.UnityEngine.UI.Text
local Vector3 = CS.UnityEngine.Vector3
local Vector2 = CS.UnityEngine.Vector2
local Color = CS.UnityEngine.Color
local Button = CS.UnityEngine.UI.Button

local AStar = {}

function AStar:init()
    self.canvas = GameObject.Find("Canvas")
    self.ground = self.canvas.transform:Find("ground")
    self.point = self.ground:Find("point")
    self.point.gameObject:SetActive(false)
    self:create_map()
    self:add_click_event()
    -- self:do_serach()
end

function AStar:button_step()
    local button = self.canvas.transform:Find("Button"):GetComponent(typeof(Button))
    button.onClick:AddListener(function()
        self:search_step()
    end)
end

function AStar:add_click_event()
    self.cur_set = "start"
    for k,v in pairs(self.points) do
        v.btn.onClick:AddListener(function()
            -- if self.cur_set == "start" then
            --     self.cur_set = "end"
            --     local start_name = v.pos.x .. '-' .. v.pos.y
            --     local id, start_point = self:find_point(start_name, self.points)
            --     self.start_point = start_point
            --     self.start_point.img.color = Color.green
            -- elseif self.cur_set == "end" then
                for k1, v1 in pairs(self.points) do
                    v1.img.color = Color.white
                end
                local end_name = v.pos.x .. '-' .. v.pos.y
                local id, end_point = self:find_point(end_name, self.points)
                self.end_point = end_point
                self.end_point.img.color = Color.red
                self.cur_set = "start"
                self:do_serach()
            -- end
        end)
    end
end

function AStar:create_map()
    -- 27 * 15
    self.startPos = {14, 9}
    self.endPos = {22, 5}
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
        local button = pointObj.transform:Find("Button"):GetComponent(typeof(Button))
        
        local point = {
            name = name,
            pos = {x = x, y = y},
            G = 0,
            H = 0,
            F = 9999,
            tex_F = tex_F,
            tex_H = tex_H,
            tex_G = tex_G,
            img = img,
            btn = button,
            root = nil,
        }
        
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
        if y == 12 and x >= 7 and x <= 19 or 
           y == 4 and x >= 7 and x <= 19 or
           x == 19 and y >= 4 and y <= 12
        then
            table.insert(self.wall, point)
        else
            table.insert(self.points, point)
        end
    end

    for k,v in pairs(self.wall) do
        v.img.color = Color.black
    end

    local start_name = self.startPos[1] .. '-' .. self.startPos[2]
    local id, start_point = self:find_point(start_name, self.points)
    self.start_point = start_point
    local end_name = self.endPos[1] .. '-' .. self.endPos[2]
    local id, end_point = self:find_point(end_name, self.points)
    self.end_point = end_point
    self.start_point.img.color = Color.green
    self.end_point.img.color = Color.red

end

function AStar:do_serach()
    self.open_list = {}
    self.close_list = {}
    self.start_point.G = 0
    self.start_point.H = math.abs(self.end_point.pos.x - self.start_point.pos.x) + math.abs(self.end_point.pos.y - self.start_point.pos.y)
    self.start_point.F = self.start_point.G + self.start_point.H
    table.insert(self.open_list, self.start_point)
    self.cur_point = self.open_list[1]

    while #self.open_list > 0 and self.cur_point.name ~= self.end_point.name do
        self:search_child()
    end
    self:show_path()
    -- self:button_step()
end

function AStar:search_step()
    if self.finish then
        return
    end
    self:search_child()
    if self.cur_point.name == self.end_point.name then
        self:show_path()
        self.finish = true
    end
end

function AStar:show_path()
    local p = self.end_point
    while p.root do
        p.img.color = Color.green
        p = p.root
    end
end

function AStar:search_child()
    table.sort(self.open_list, function(a, b)
        if a.F == b.F then
            return a.H < b.H
        end
        return a.F < b.F
    end)
    self.cur_point = table.remove(self.open_list, 1)
    self.cur_point.img.color = Color.yellow
    table.insert(self.close_list, self.cur_point)
    self:child_point(0, 1)
    self:child_point(1, 0)
    self:child_point(0, -1)
    self:child_point(-1, 0)
end

function AStar:child_point(dx, dy)
    local name = (self.cur_point.pos.x + dx) .. "-" .. (self.cur_point.pos.y + dy)
    local id, point = self:find_point(name, self.points)
    if not point then
        return
    end
    local close_id, p = self:find_point(name, self.close_list)
    if close_id and p then
        return
    end

    self:caculate_fgh(point)
    local open_id, p = self:find_point(name, self.open_list)
    if not open_id then
        table.insert(self.open_list, point)
    end
    point.img.color = Color.blue
end

function AStar:caculate_fgh(point)
    local g = self.cur_point.G + 1
    local h = math.abs(self.end_point.pos.x - point.pos.x) + math.abs(self.end_point.pos.y - point.pos.y)
    if point.F > h + g then
        point.G = g
        point.H = h
        point.F = h + g
        point.root = self.cur_point
    end
    point.tex_F.text = string.format("%.0f", point.F)
    point.tex_G.text = string.format("%.0f", point.G)
    point.tex_H.text = string.format("%.0f", point.H)
end

function AStar:find_point(name, list)
    for k,v in pairs(list) do
        if v.name == name then
            return k, v
        end
    end
    return nil
end

AStar:init()
