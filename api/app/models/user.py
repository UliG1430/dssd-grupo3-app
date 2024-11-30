from app import db

class User(db.Model):
    __tablename__ = 'users'

    # Columns
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    mail = db.Column(db.String(120), unique=True, nullable=False)
    username = db.Column(db.String(50), unique=True, nullable=False)
    password = db.Column(db.String(200), nullable=False)
    rol = db.Column(db.String(1), nullable=False, default='A')  # Nueva columna para el rol

    # Relationship with 'Orden'
    ordenes = db.relationship('Orden', backref='user', lazy=True)

    def __repr__(self):
        return f"<User {self.username}, Rol: {self.rol}>"