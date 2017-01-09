function Setup-Env
{
	(Get-Process -Id $pid).priorityclass = 'Idle'
	return (Get-Process -Id $pid).priorityclass
}