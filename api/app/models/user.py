from app import db

class User(db.Model):
    __tablename__ = 'users'

    # Columns
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    mail = db.Column(db.String(120), unique=True, nullable=False)  # User's email address
    username = db.Column(db.String(50), unique=True, nullable=False)  # Username for login
    password = db.Column(db.String(200), nullable=False)  # Hashed password

    # Relationship with 'Orden'
    ordenes = db.relationship('Orden', backref='user', lazy=True)

    def __repr__(self):
        return f"<User {self.username}>"
