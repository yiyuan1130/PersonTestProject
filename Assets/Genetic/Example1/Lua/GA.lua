local GA = {}

GA.DNA_SIZE = 0
GA.POP_SIZE = 0
GA.CROSSOVER_RATE = 0
GA.MUTATION_RATE = 0
GA.GENERATIONS = 0

GA.POPULATIONS = {}

GA.init = function(dna_size, pop_size, crossover_rate, mutation_rate, generations)
    GA.DNA_SIZE = dna_size
    GA.POP_SIZE = pop_size
    GA.CROSSOVER_RATE = crossover_rate
    GA.MUTATION_RATE = mutation_rate
    GA.GENERATIONS = generations
    for i = 1, GA.POP_SIZE do
        local dna = {}
        for j = 1, GA.DNA_SIZE do
            dna[j] = j
        end
        for i = 1, #dna do
            local rid = math.random(i, #dna)
            dna[rid], dna[i] = dna[i], dna[rid]
        end
        table.insert(GA.POPULATIONS, dna)
    end
end

GA.translateDNA = function(populations, points)
    local point_orders = {}
    for idx, dna in ipairs(populations) do
        local order = {}
        for _, pIdx in ipairs(dna) do
            local p = points[pIdx]
            table.insert(order, p)
        end
        table.insert(point_orders, order)
    end
    return point_orders
end

GA.get_fitness = function(point_orders)
    local fitness_map = {}
    local dis_sum = 0
    local dis_min = 9999999999
    local dis_max = 0
    local orders = {}
    for idx, order in ipairs(point_orders) do
        local dis = 0
        for i = 1, #order - 1 do
            dis = dis + Util.get_dis(order[i], order[i + 1])
        end
        dis = dis + Util.get_dis(order[#order], order[1])
        table.insert(fitness_map, {pop_id = idx, dis = dis})
    end
    return fitness_map
end

GA.selection = function(fitness_map)
    local populations = {}
    table.sort(fitness_map, function(a, b)
        return a.dis < b.dis
    end)
    -- for i = 1, GA.POP_SIZE do
    --     item = fitness_map[i]
    --     if math.random() < 1000 / GA.POP_SIZE then
    --         table.insert(populations, GA.POPULATIONS[item.pop_id])
    --     end
    -- end
    for i = 1, GA.POP_SIZE do
        item = fitness_map[i]
        table.insert(populations, GA.POPULATIONS[item.pop_id])
    end
    return populations
end

GA.crossover = function(parent)
    local child = nil
    if math.random() < GA.CROSSOVER_RATE then
        child = {}
        local parent1 = parent
        local parent2 = GA.POPULATIONS[math.random(1, GA.POP_SIZE)]
        local dna1 = {}
        local dna2 = {}
        for i = 1, GA.DNA_SIZE do
            dna1[i] = parent1[i]
            dna2[i] = parent2[i]
        end
        -- for i = 1, GA.DNA_SIZE do
        --     if i < GA.DNA_SIZE / 2 then
        --         table.insert(child, dna1[i])
        --     else
        --         table.insert(child, dna2[i])
        --     end
        -- end
        for i = 1, GA.DNA_SIZE do
            if i % 2 == 1 then
                table.insert(child, dna1[i])
            end
        end
        for i = GA.DNA_SIZE, 1, -1 do
            for k,v in pairs(child) do
                if dna2[i] == v then
                    table.remove(dna2, i)
                end
            end
        end
        for i, v in ipairs(dna2) do
            table.insert(child, v)
        end
    end
    return child
end

GA.mutation = function(child)
    if math.random() < GA.MUTATION_RATE then
        local idxs = Util.random_get_idx(GA.DNA_SIZE, 2)
        child[idxs[1]], child[idxs[2]] = child[idxs[2]], child[idxs[1]]
    end
    return child
end

GA.evolve = function(fitness)
    local populations = GA.selection(fitness)
    local pop_copy = {}
    for k, v in ipairs(populations) do
        pop_copy[k] = v
    end
    local childs = {}
    for _, parent in pairs(populations) do
        local otherP = populations[math.random(1, GA.POP_SIZE)]
        local child = GA.crossover(parent)
        if child then
            -- child = GA.mutation(child)
            table.insert(childs, child)
        end
    end
    for k, v in pairs(populations) do
        v = GA.mutation(v)
    end
    for k, v in pairs(childs) do
        table.insert(populations, v)
    end
    GA.POPULATIONS = populations
    -- GA.POP_SIZE = #populations
end

return GA