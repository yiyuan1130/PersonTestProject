local RandomPoint = {}

RandomPoint.X_RANG = {-8.5, 8.5}
RandomPoint.Y_RANG = {-3.5, 3.5}

RandomPoint.SetupPoint = function(count)
    local points = {}
    local pointParent = GameObject.Find("points").transform
    local pointPrefab = pointParent:Find("point").gameObject
    pointPrefab.gameObject:SetActive(false)
    local xRange = RandomPoint.X_RANG[2] - RandomPoint.X_RANG[1]
    local yRange = RandomPoint.Y_RANG[2] - RandomPoint.Y_RANG[1]
    for i = 1, count do
        local pointObj = GameObject.Instantiate(pointPrefab)
        pointObj:SetActive(true)
        pointObj.name = "point_" .. i
        pointObj.transform:SetParent(pointParent)
        local x = math.random() * xRange - RandomPoint.X_RANG[2]
        local y = math.random() * yRange - RandomPoint.Y_RANG[2]
        local sin = Mathf.Sin(i / count * math.pi * 2) * 4.7
        local cos = Mathf.Cos(i / count * math.pi * 2) * 4.7
        -- x, y = sin, cos
        pointObj.transform.position = Vector3(x, y, 0)
        -- points[i] = {x = x, y = y}
        points[i] = {x, y}
    end
    return points
end

return RandomPoint