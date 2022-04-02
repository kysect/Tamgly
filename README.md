# Tamgly. Yet another time tracker

## Мотивация

Цель: повысить качество планирования и управления задачами

Задачи:

1. Выявлять задачи, которые заняли больше времени чем планировалось и другие проблемы, которые создают расхождение между эстимейтами и реально затраченным временем
2. Автоматизация подсчёта необходимого времени на выполнение задачи с учётом порядка выполнения и рабочих часов
3. Отслеживание нагрузки на день, неделю и месяц для выявления ситуаций, когда запланировано больше задач чем есть доступного времени
4. Подсчёт суммарных эстимейтов по группе задач с учётом блокировок и возможностей параллельно выполнять
5. Построение зависимости времени выполнения задачи от её приоритета

## План работ (MVP)

1. Управление задачами и проектами. Возможность создавать в системе задачи, отмечать их как выполненные.  Интеграция с MS Todo для возможности просмотра задачи там.
2. Отслеживание времени выполнения задачи. Интеграция с Clockify.
3. Возможность задавать эстимейты по задача, просматривать информацию о расхождении эстимейтов и затраченного времени.
4. Беклоги задач на день, неделю или месяц, отслеживание выпавших задач, ситуаций, когда на задачу уже не хватает времени
5. Чеклисты (дейли, викли) со списком задач, которые нужно не забывать выполнить регулярно. Поддержка регулярных событий как задач, учёт их в подсчёте эстимейтов
6. Приоритеты задач, блокирующие задачи, подбор задач по приоритетам и блокировка. Возможность указывать, что задача выполняется внешним исполнителе (для того, чтобы явно указать, что другая задача этим заблокирована
7. Возможность задавать количество часов которое должно выделяться на задачу или проект.
8. Алгоритм оценивания эстимейтов с учётом выбранного приоритета, который позволит понять когда может быть выполнена задача с учётом текущей нагрузки и приоритета задачи.

## Сущности

- Задача (оно же та... Oh wait, нельзя в шарпе таски тасками называть). Описывает необходимость выполнить какую-то активность. Описывается заголовком, деталями, идентификатором, статусом (выполнена ли), назанченным сотрудником, списком блокирующих задач, принадлежностью к проекту, эстимейтами, затраченны временем.
- Временной интервал - зарегестрированный в системе отрезок времени в которым выполнялась какая-то активность. Ассоциирован с задачами, задачи агреггируют такие интервалы.
- Митинг (или как-то обыграть интервалы) - записи в календаре как задачи
- Проект - группа задач. Может иметь лимит времени, которое должно быть затрекано за определённый интервал - неделю или месяц.
- Эстимейт - время, которое планируется потратить на выполнение
- Приоритет - числовое значение, которое описывает порядок выполнения. Можем быть в интервале от 1 до 5. Задачи с 1 приоритетом должны выполнять раньше, чем с 5. Если задача с 3 приоритетом блокирует задачу 1 приоритета, то она должна считаться также 1 приоритетом.

## Технические детали

Прототип планируется реализовать как приложение для одного пользователя, без поддержки команд и шаринга. Реализовавываться будет в качестве консольного приложения.
