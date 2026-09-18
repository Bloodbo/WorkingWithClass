namespace WorkingWithClass;
using System.Text.Json;
using System.Text;
internal class Program
{
	const string Path = "ItemsData.json";
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		WriteIndented = true,  // красивый вывод с отступами
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // для русских букв
		IncludeFields = true   // сериализовать поля (у тебя в Items поля, а не свойства)
	};
	private static void Main(string[] args)
	{
		List<Items> items = new List<Items>();
		CallMenu(items);
	}
	public static void PrintAll(List<Items> items)
	{
		if (items != null)
		{
			items.ForEach(item => item.PrintItemsInfo());
		}
		/*
		 items — список предметов.
		ForEach — метод, который обходит каждый элемент списка.
		item => — лямбда-выражение, где item — это один элемент из списка (по очереди).
		=> — «для этого элемента сделай следующее».
		item.PrintItemsInfo() — что именно сделать (вызвать метод у этого элемента).
		 */
	}
	public static void CallMenu(List<Items> items)
	{
		bool flag = true;
		while (flag == true)
		{
			Console.WriteLine("1 - начать добавление объекта\n2 - выход" +
			"\n3 - вывод всех\n4 - поиск по названию объекта\n5 - удаление по названию" +
			"\n6 - отсортировать по названию\n7 - статистика по создателям" +
			"\n8 - редактировать предметыт\n9 -	сортировка по количеству" +
			"\n10 - сохранение в JSON файл списка");
			string answerChoiceUser = ReadLine();
			if (CheckIntTryParse(answerChoiceUser, out int ProofChoice))
			{
				switch (ProofChoice)
				{
					case 1:
						{
							InPutItems(items);
							break;
						}
					case 2:
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Выход из программы.");
							flag = false;
							break;
						}
					case 3:
						{
							
							PrintAll(items);
							break;
						}
					case 4:
						{
							Console.WriteLine("введите строку для поиска по названию сущности: ");
							string inputUserItem = ReadLine();
							Console.WriteLine($"результат поиска:\n");
							var resultItems = SearchNameItem(items, inputUserItem);
							if (resultItems.Count == 0)
							{
								Console.WriteLine("Ничего не найдено");
							}
							else
							{
								PrintAll(resultItems);
							}
							break;
						}
					case 5:
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("введите названия объекта для удаление: ");
							string inputUserRequest = ReadLine();
							string result = DeleteItem(items, inputUserRequest);
							Console.WriteLine($"{result}");
							PrintAll(items);
							break;
						}
					case 6:
						{
							Console.WriteLine("отсортированный список:\n");
							PrintAll(SortByName(items));
							break;
						}
					case 7:
						{
							Console.WriteLine("Статистика по создателям:\n");
							List<string> CreatorList = StatisticsCreator(items);
							foreach(var creator in CreatorList)
							{
								Console.WriteLine($"{creator}");
							}
							break;
						}
					case 8:
					{
							Console.WriteLine("введите название элемента которое хотите отредактировать: ");
							string answerItem = ReadLine();
							if(CheckStr(answerItem))
							{
								EditItem(items,answerItem);
							}
							break;
					}
					case 9:
					{
							break;
						}
					case 10:
					{
						if(items != null)
						{
								SaveForJSON(items);
						}
							break;
					}
					default:
						{
							Console.WriteLine($"{ProofChoice} - отсутствует в меню");
							break;
						}
				}
			}
		}
	}
	public static void EditItem(List<Items> items, string input)
	{
		var check = items.FirstOrDefault(item => item.nameItem.Equals(input, StringComparison.OrdinalIgnoreCase));
		if (check != null)
		{
			bool flag = true;
			while (flag == true)
			{
				Console.WriteLine("введите:\n1 - изменить названия" +
				"\n2 - изменить название создателя" +
				"\n3 - изменить описание объекта" +
				"\n4 - изменить кол-во" +
				"\n0 - выход");
				string choiceEdit = ReadLine();
				if (CheckStr(choiceEdit) && CheckIntTryParse(choiceEdit, out int choiceProof))
				{
					switch (choiceProof)
					{
						case 1:
							{
								Console.WriteLine("Введите новое имя: ");
								string ChangedName = ReadLine();
								if (CheckStr(ChangedName))
								{
									check.nameItem = ChangedName;
								}
								Console.WriteLine("Название изменено.");
								break;
							}
						case 2:
							{
								Console.WriteLine("Введите нового создателя объекта: ");
								string ChangedCreator = ReadLine();
								if (CheckStr(ChangedCreator))
								{
									check.creatorItem = ChangedCreator;
								}
								Console.WriteLine("Создатель изменён.");
								break;
							}
						case 3:
							{
								Console.WriteLine("Введите новое описание: ");
								string ChangedDescription = ReadLine();
								if (CheckStr(ChangedDescription))
								{
									check.description = ChangedDescription;
								}
								Console.WriteLine("Описание изменено.");
								break;
							}
						case 4:
							{
								Console.WriteLine("введите новое значение количество: ");
								string ChangedAmount = ReadLine();
								if (CheckStr(ChangedAmount) && CheckIntTryParse(ChangedAmount, out int newAmountItem))
								{
									check.amountItems = newAmountItem;
								}
								Console.WriteLine("Количество изменено.");
								break;
							}
						case 0:
							{
								Console.WriteLine("выход из подменю редактирования");
								flag = false;
								break;
							}
						default:
							{
								break;
							}
					}
				}
			}
		}
		else
		{
			Console.WriteLine("Предмет не найден.");
		}
	}
	public static void SaveForJSON(List <Items> items)
	{
		string json = JsonSerializer.Serialize(items, JsonOptions);
		File.WriteAllText(Path, json, Encoding.UTF8);
	}
	public static List<Items> SortByName(List<Items> items)
	{
		var SortList = items.OrderBy(n => n.nameItem).ToList();
		// Метод OrderBy в LINQ упорядочивает элементы коллекции по возрастанию на основе заданного ключа
		return SortList;
	}
	public static List<string> StatisticsCreator(List<Items> items)
	{
		// Метод GroupBy() принимает лямбда-выражение, которое определяет, по какому свойству (ключу) объединять объекты. 
		/*
		 * Когда ты используешь GroupBy, ты получаешь коллекцию групп. Каждая группа — это как коробка с предметами, у которой есть:
           Ключ (например, имя создателя).
           Коллекция предметов, которые относятся к этому ключу.
		 */
		var result = new List<string>();
		foreach (var group in items.GroupBy(item => item.creatorItem))
		{
			result.Add($"{group.Key}: {group.Count()} предметов");
		}
		return result;
	}
	public static string DeleteItem(List<Items> items, string input)
	{
		var foundation = items.FirstOrDefault(item => item.nameItem.Equals(input, StringComparison.OrdinalIgnoreCase));
		if (foundation != null)
		{
			items.Remove(foundation);
			return "удаление завершено";
		}
		return $"объект {input} не найден";
	}
	public static List<Items> SearchNameItem(List<Items> items, string input)
	{
		if (CheckStr(input))
		{
			return items.Where(item => item.nameItem.ToLower().IndexOf(input.ToLower()) >= 0).ToList();
		}
		return new List<Items>();
	}
	public static void InPutItems(List<Items> items)
	{
		Console.WriteLine("введите название сущности: ");
		string nameItem = ReadLine();
		Console.WriteLine("введите описание сущности: ");
		string description = ReadLine();
		Console.WriteLine("введите количество = ");
		string amountItems = ReadLine();
		Console.WriteLine("введите создателя: ");
		string creatorItem = ReadLine();
		if (CheckStr(nameItem)
		&& CheckStr(description) && CheckStr(creatorItem)
		&& CheckIntTryParse(amountItems, out int count))
		{
			Items item = new Items(nameItem, description, count, creatorItem);
			item.PrintItemsInfo();
			items.Add(item);
		}
	}
	public static bool CheckStr(string InPut)
	{
		if (!string.IsNullOrEmpty(InPut) && InPut.Length >= 3 && !string.IsNullOrWhiteSpace(InPut))
		{
			return true;
		}
		return false;
	}
	public static bool CheckIntTryParse(string InPut, out int result)
	{
		result = 0;
		if (!string.IsNullOrEmpty(InPut))
		{
			if (int.TryParse(InPut, out int AmountItems) && AmountItems > 0)
			{
				return true;
			}
		}
		return false;
	}
	public static string ReadLine()
	{
		return Console.ReadLine()?.Trim() ?? "";
	}
}