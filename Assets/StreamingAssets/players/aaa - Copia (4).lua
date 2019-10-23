import 'System'
import 'UnityEngine'
import 'Assembly-CSharp'	-- The user-code assembly generated by Unity

local invalidPosition = Vector3.zero + Vector3.up * 10
local position = Vector3.zero

function Start()
	this:SetNick('Duarte Bernárdez')
	this:SetColor('Blue')
end

function Update()
	if this:GetNearestTower() ~= invalidPosition then
		this:MoveTo(this:GetNearestTower())
	end
end