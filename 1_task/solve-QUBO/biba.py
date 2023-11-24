def get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l):
    return i * num_j * num_k * num_l + j * num_k * num_l + k * num_l + l

def generate_qubo_matrix(num_i, num_j, num_k, num_l, weights):
    qubo_size = num_i * num_j * num_k * num_l
    qubo = [[0 for _ in range(qubo_size)] for _ in range(qubo_size)]

    # Ограничение количества лекций в день для каждой группы
    for i in range(num_i):
        for k in range(num_k):
            for l in range(num_l):
                daily_lectures = sum(qubo[get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)][get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)] for j in range(num_j))
                qubo[get_variable_index(i, 0, k, l, num_i, num_j, num_k, num_l)][get_variable_index(i, 0, k, l, num_i, num_j, num_k, num_l)] += weights['daily_lectures'] * (6 - daily_lectures)

    # Учет выходных дней преподавателей
    days_off = {
        'n1': 3,  # Среда
        'n2': 1,  # Понедельник
        'n3': 6,  # Суббота
        'n4': 2,  # Вторник
        'n5': 2,  # Вторник
    }
    for teacher, day in days_off.items():
        for k in range(num_k):
            var_index = get_variable_index(int(teacher[1]) - 1, 0, k, day, num_i, num_j, num_k, num_l)
            qubo[var_index][var_index] += weights['days_off']

    # Ограничение количества занятий по одному предмету для группы
    for j in range(num_j):
        for k in range(num_k):
            for l in range(num_l):
                for i in range(num_i):
                    for i_prime in range(num_i):
                        for l_prime in range(num_l):
                            if i_prime != i or l_prime != l:
                                var_index = get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)
                                var_index_prime = get_variable_index(i_prime, j, k, l_prime, num_i, num_j, num_k, num_l)
                                qubo[var_index][var_index_prime] += weights['same_subject'] * 2

    # Ограничение часов работы преподавателя в неделю
    for i in range(num_i):
        total_hours = 0
        for j in range(num_j):
            for k in range(num_k):
                for l in range(num_l):
                    var_index = get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)
                    total_hours += qubo[var_index][var_index]
        if total_hours > weights['max_hours_per_week']:
            for l in range(num_l):
                var_index = get_variable_index(i, 0, 0, l, num_i, num_j, num_k, num_l)
                qubo[var_index][var_index] += weights['weekly_hours_limit']

    # Ограничение часов занятий для каждого предмета в группе
    for j in range(num_j):
        for i in range(num_i):
            lectures_per_subject = sum(qubo[get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)][get_variable_index(i, j, k, l, num_i, num_j, num_k, num_l)] for k in range(num_k) for l in range(num_l))
            qubo[get_variable_index(i, j, 0, 0, num_i, num_j, num_k, num_l)][get_variable_index(i, j, 0, 0, num_i, num_j, num_k, num_l)] += weights['lectures_per_subject'] * (4 - lectures_per_subject)

    # Привязка преподавателей к предметам
    teachers_subjects = {
        0: [0, 1],  # Преподаватель 0 преподаёт предметы 0 и 1
        1: [2, 3],  # Преподаватель 1 преподаёт предметы 2 и 3
        2: [4, 5],
        3: [6, 7],
        4: [8, 9]
        # Добавьте привязки для остальных преподавателей и предметов
    }

    # Добавление связей преподавателей с их предметами в матрицу QUBO
    for teacher, subjects in teachers_subjects.items():
        for subject in subjects:
            for k in range(num_k):
                for l in range(num_l):
                    var_index = get_variable_index(teacher, subject, k, l, num_i, num_j, num_k, num_l)
                    qubo[var_index][var_index] += weights['teacher_subject_relation']

    return qubo

# Параметры
num_teachers = 5
num_subjects = 10
num_groups = 2
num_days = 7

# Веса для каждого ограничения
weights = {
    'daily_lectures': 10,
    'days_off': 5,
    'same_subject': 3,
    'max_hours_per_week': 4,
    'weekly_hours_limit': 2,
    'lectures_per_subject': 10,
    'teacher_subject_relation': 5,  # Вес связи преподавателя с предметом
}

# Создание матрицы QUBO
qubo_matrix = generate_qubo_matrix(num_teachers, num_subjects, num_groups, num_days, weights)

# Вывод матрицы (для небольших значений переменных)
for row in qubo_matrix:
    print(row)
