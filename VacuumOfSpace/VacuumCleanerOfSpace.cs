using System;
using UnityEngine;

namespace VacuumOfSpace
{
	// Token: 0x02000002 RID: 2
	internal class VacuumCleanerOfSpace : StateMachineComponent<ColdBreather.StatesInstance>, ISim1000ms
	{
		public void Sim1000ms(float dt)
		{
			this.VacuumTheVacuum();
		}

		private void VacuumTheVacuum()
		{
			int[,] array = new int[Grid.WidthInCells, Grid.HeightInCells];
			int[,] invalidGasCells = new int[Grid.WidthInCells, Grid.HeightInCells];
			int num = 0;
			while ((float)num < Grid.HeightInMeters)
			{
				int num2 = 0;
				while ((float)num2 < Grid.WidthInMeters)
				{
					int num3 = Grid.PosToCell(new Vector3((float)num2, (float)num));
					if (Grid.IsCellOpenToSpace(num3))
					{
						int cellInDirection = Grid.GetCellInDirection(num3, Direction.Down);
						if (!Grid.IsCellOpenToSpace(cellInDirection) && Grid.IsGas(cellInDirection))
						{
							this.PullMyHairAndCallMeRecursive(array, num2, num, cellInDirection, 15f, 0f, invalidGasCells, 0);
						}
					}
					num2++;
				}
				num++;
			}
			int num4 = 0;
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				num4++;
				for (int j = 0; j < Grid.HeightInCells; j++)
				{
					num4++;
					if (array[i, j] != 0 && num4 % 5 != 0)
					{
						SimMessages.ReplaceElement(array[i, j], (SimHashes)758759285, CellEventLogger.Instance.SandBoxTool, 0f, 0f, byte.MaxValue, 0, -1);
					}
				}
			}
		}

		private unsafe int PullMyHairAndCallMeRecursive(int[,] foundGasCells, int x, int y, int currentCell, float limitMass, float massToConsume, int[,] invalidGasCells, int skipCells)
		{
			if (y < 0)
			{
				return 0;
			}
			if (y >= Grid.HeightInCells)
			{
				return 0;
			}
			if (x < 0)
			{
				return 0;
			}
			if (x >= Grid.WidthInCells)
			{
				return 0;
			}
			if (massToConsume > limitMass)
			{
				return 0;
			}
			if (skipCells == 0)
			{
				if (Grid.IsValidCell(currentCell))
				{
					float num = Grid.mass[currentCell];
					if (num > 0.01f)
					{
						foundGasCells[x, y] = currentCell;
						massToConsume += num;
					}
					else
					{
						skipCells = 15;
						invalidGasCells[x, y] = currentCell;
					}
				}
				else
				{
					skipCells = 5;
					invalidGasCells[x, y] = currentCell;
				}
			}
			else
			{
				invalidGasCells[x, y] = currentCell;
				skipCells--;
			}
			try
			{
				int cellInDirection = Grid.GetCellInDirection(currentCell, Direction.Left);
				if (Grid.IsGas(cellInDirection) && foundGasCells[x - 1, y] == 0 && invalidGasCells[x - 1, y] == 0)
				{
					skipCells = this.PullMyHairAndCallMeRecursive(foundGasCells, x - 1, y, cellInDirection, limitMass, massToConsume, invalidGasCells, skipCells);
				}
				int cellInDirection2 = Grid.GetCellInDirection(currentCell, Direction.Up);
				if (Grid.IsGas(cellInDirection2) && foundGasCells[x, y + 1] == 0 && invalidGasCells[x, y + 1] == 0)
				{
					skipCells = this.PullMyHairAndCallMeRecursive(foundGasCells, x, y + 1, cellInDirection2, limitMass, massToConsume, invalidGasCells, skipCells);
				}
				int cellInDirection3 = Grid.GetCellInDirection(currentCell, Direction.Right);
				if (Grid.IsGas(cellInDirection3) && foundGasCells[x + 1, y] == 0 && invalidGasCells[x + 1, y] == 0)
				{
					skipCells = this.PullMyHairAndCallMeRecursive(foundGasCells, x + 1, y, cellInDirection3, limitMass, massToConsume, invalidGasCells, skipCells);
				}
				int cellInDirection4 = Grid.GetCellInDirection(currentCell, Direction.Down);
				if (Grid.IsGas(cellInDirection4) && foundGasCells[x, y - 1] == 0 && invalidGasCells[x, y - 1] == 0)
				{
					skipCells = this.PullMyHairAndCallMeRecursive(foundGasCells, x, y - 1, cellInDirection4, limitMass, massToConsume, invalidGasCells, skipCells);
				}
			}
			catch
			{
			}
			return skipCells;
		}
	}
}
