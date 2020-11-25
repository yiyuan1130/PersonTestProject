local Util = {}
Util.get_dis = function(p1, p2)
    local dx = p1[1] - p2[1]
    local dy = p1[2] - p2[2]
    return math.sqrt(dx * dx + dy * dy)
end

Util.deep_copy = function(t)
    local new_t = {}
    for k, v in pairs(t) do
        if type(v) == "table" then
            new_t[k] = Util.deep_copy(v)
        else
            new_t[k] = v
        end
    end
    return new_t
end

Util.random_get_idx = function(size, count)
    local t = {}
    for i = 1, size do
        t[i] = i
    end
    for i = 1, count do
        local ridx = math.random(i, size)
        t[i], t[ridx] = t[ridx], t[i]
    end
    local ret = {}
    for i = 1, count do
        ret[i] = t[i]
    end
    return ret
end

return Util
