﻿using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class ObjectComparison
	{
		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();

			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

			actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
					.Excluding(person => person.SelectedMemberInfo.Name.Equals(nameof(Person.Id)))
					.AllowingInfiniteRecursion());
		}

		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

			// Какие недостатки у такого подхода?
			// Нет подробного описания в выбрасываемой ошибке,
			// нельзя сразу определить какие из свойств не равны.
			// Это рекурсивный способ, а значит стек может переполнится
			// и выбросит stackOverflowException.
			// Не так удобно читать код, нужно искать метод чтобы понять как он работает.
			// Можно перепутать местами переменные.
			// Нужно добовлять дополнительные проверки при изменении класса Person.
			Assert.True(AreEqual(actualTsar, expectedTsar));
		}

		private bool AreEqual(Person? actual, Person? expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
				actual.Name == expected.Name
				&& actual.Age == expected.Age
				&& actual.Height == expected.Height
				&& actual.Weight == expected.Weight
				&& AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person? Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person? parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}