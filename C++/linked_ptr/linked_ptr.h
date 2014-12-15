#pragma once

#include <algorithm>
#include <stddef.h>

namespace smart_ptr 
{
	template<class T>
	struct linked_ptr
	{
	public:
		linked_ptr() : p_(nullptr), prev(nullptr), next(nullptr)
		{}

		explicit linked_ptr(T* object) : p_(object), prev(nullptr), next(nullptr)
		{}

		linked_ptr(linked_ptr const& ptr) :p_(ptr.get())
		{insert(ptr);}

		template<class U>
		explicit linked_ptr(U* object) : p_(object), prev(nullptr), next(nullptr)
		{}

		template<class U>
		linked_ptr(linked_ptr<U> const& ptr) : p_(ptr.get())
		{insert((linked_ptr&)ptr);}

		~linked_ptr()
		{
			enum { must_be_a_comlpete_type = sizeof(T) };

			if (prev != nullptr)
				{prev->next = next;}
			if (next != nullptr)
				{next->prev = prev;}
			if (next == nullptr && prev == nullptr)
				{delete p_;}
		}

		linked_ptr& operator=(linked_ptr const& x)
		{
			linked_ptr temp(x);
			swap(temp);
			return *this;
		}

		template<class U>
		linked_ptr<T>& operator=(linked_ptr<U> const& x)
		{
			linked_ptr<T> temp(x);
			swap(temp);
			return *this;
		}

		void reset()
		{
			if (next == nullptr && prev == nullptr)
				{delete p_;}
			next = prev = nullptr;
			p_ = nullptr;
		}

		void reset(T* x)
		{
			linked_ptr<T> p(x);
			swap(p);
		}

		template<class U>
		void reset(U* x)
		{			
			linked_ptr<U> p(x);
			swap(p);
		}


		void swap(linked_ptr& x)
		{
			bool left = prev == &x;
			bool right = next == &x;

			std::swap(prev, x.prev);
			std::swap(next, x.next);

			if (left)
			{
				next = &x;
				x.prev = this;
			}
			else if (right)
			{
				prev = &x;
				x.next = this;
			}


			this->insert_this();
			x.insert_this();

			std::swap(p_, x.p_);
		}

		T* get() const
		{return p_;}

		bool unique() const
		{
			return (prev == nullptr && next == nullptr && p_ != nullptr);
		}

		T* operator->() const
		{return get();}

		T& operator*() const
		{return *get();}

		explicit operator bool() const
		{return p_ != nullptr;}

	private:
		T* p_;
		mutable linked_ptr const* prev;
		mutable linked_ptr const* next;

		void insert(linked_ptr const& x)
		{
			next = x.next;
			prev = &x;
			if (x.next != nullptr)
				{x.next->prev = this;}
			x.next = this;
		}
		void insert_this() {
			if (prev) {
				prev->next = this;
			}
			if (next) {
				next->prev = this;
			}
		}
	};
	
	template<class T>
	inline void swap(linked_ptr<T>& a, linked_ptr<T>& b)
	{a.swap(b);}

	template<class T1, class T2>
	inline bool operator==(const linked_ptr<T1>& a, const linked_ptr<T2>& b)
	{return a.get() == b.get();}

	template<class T>
	inline bool operator==(const linked_ptr<T>& a, std::nullptr_t)
	{return a.get() == nullptr;}

	template<class T>
	inline bool operator==(std::nullptr_t, const linked_ptr<T>& b)
	{return b.get() == nullptr;}

	template<class T, class U>
	inline bool operator!=(const linked_ptr<T>& a, const linked_ptr<U>& b)
	{return !(a == b);}

	template<class T>
	inline bool operator!=(const linked_ptr<T>& a, std::nullptr_t)
	{return !(a == nullptr);}

	template<class T>
	inline bool operator!=(std::nullptr_t, const linked_ptr<T>& b)
	{return !(nullptr == b);}

	template<class T, class U>
	inline bool operator<(const linked_ptr<T>& a, const linked_ptr<U>& b)
	{return a.get() < b.get();}

	template<class T>
	inline bool operator<(const linked_ptr<T>& a, std::nullptr_t)
	{return a.get() < nullptr;}

	template<class T>
	inline bool operator<(std::nullptr_t, const linked_ptr<T>& b)
	{return b.get() < nullptr;}

	template<class T, class U>
	inline bool operator>(const linked_ptr<T>& a, const linked_ptr<U>& b)
	{return a.get() > b.get();}

	template<class T>
	inline bool operator>(const linked_ptr<T>& a, std::nullptr_t)
	{return a.get() > nullptr;}

	template<class T>
	inline bool operator>(std::nullptr_t, const linked_ptr<T>& b)
	{return b.get() > nullptr;}

	template<class T1, class T2>
	inline bool operator<=(const linked_ptr<T1>& a, const linked_ptr<T2>& b)
	{return !(a > b);}

	template<class T>
	inline bool operator<=(const linked_ptr<T>& a, std::nullptr_t)
	{return !(a > nullptr);}

	template<class T>
	inline bool operator<=(std::nullptr_t, const linked_ptr<T>& b)
	{return !(nullptr > b);}

	template<class T, class U>
	inline bool operator>=(const linked_ptr<T>& a, const linked_ptr<U>& b)
	{return !(a < b);}

	template<class T>
	inline bool operator>=(const linked_ptr<T>& a, std::nullptr_t)
	{return !(a < nullptr);}

	template<class T>
	inline bool operator>=(std::nullptr_t, const linked_ptr<T>& b)
	{return !(nullptr < b);}

}