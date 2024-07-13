"use client";

import React, { useState, useEffect, forwardRef } from 'react';
import { Card, CardFooter, Button, useDisclosure, Select, SelectItem } from '@nextui-org/react';
import ExpenseIcon from './expense-icons';
import { MdDelete } from "react-icons/md";
import { axiosInstance } from '@/components/utilities/axiosInstance';
import { mutate } from 'swr';
import AddExpenseModal from './add-expense-modal';

import useSWRImmutable from 'swr/immutable';
import { fetcher } from '@/components/utilities/fetcher';

interface ExpensesCardProps {
    expenses: any;
    itineraryId: string;
    totalCost: number;
    setTotalCost: React.Dispatch<React.SetStateAction<number>>;
}

export const ExpensesCard = forwardRef<HTMLDivElement, ExpensesCardProps>(
    function ExpensesCard({ expenses, itineraryId, totalCost, setTotalCost }, ref) {
        const { data: currencies } = useSWRImmutable("expenses/currencies", fetcher);
        const { data: exchangeRates } = useSWRImmutable("expenses/currency-rates", fetcher);

        const [selectedCurrency, setSelectedCurrency] = useState('USD');
        const [displayTotal, setDisplayTotal] = useState(0);

        const handleDelete = (expenseId: string) => {
            axiosInstance.delete(`/expenses/${expenseId}`);
            mutate(`/itineraries/${itineraryId}`);
        }

        const { isOpen, onOpen, onOpenChange } = useDisclosure();

        useEffect(() => {
            if (expenses && exchangeRates) {
                const totalInUSD = expenses.reduce((acc: number, expense: any) => {
                    const rate = exchangeRates.find((rate: any) => rate.currencyId === expense.currencyId)?.rateToUSD || 1;
                    return acc + (expense.price / rate);
                }, 0);

                setTotalCost(Number(totalInUSD.toFixed(2)));

                const selectedRate = exchangeRates.find((rate: any) => rate.currencyId === currencies.find((c: any) => c.code === selectedCurrency)?.currencyId)?.rateToUSD || 1;
                setDisplayTotal(Number((totalInUSD * selectedRate).toFixed(2)));
            }
            // eslint-disable-next-line react-hooks/exhaustive-deps
        }, [expenses, exchangeRates, selectedCurrency, setTotalCost]);

        return (
            <Card className="relative flex flex-col min-h-[36rem] p-6 mx-5" ref={ref}>
                <div className="flex flex-row gap-4 items-center justify-between">
                    <h4 className="font-bold text-large">Budgeting</h4>
                    <Button
                        color='primary'
                        onClick={onOpen}
                    >
                        Add Expense
                    </Button>
                </div>
                <div className='flex flex-col gap-6 my-4 overflow-y-auto hide-scrollbar flex-1'>
                    {expenses?.map((expense: any) => (
                        <div key={expense.expenseId} className="flex flex-row gap-6 items-center">
                            <ExpenseIcon id={expense.expenseCategoryId} />
                            <div className="flex-auto">
                                <p>
                                    {expense.name}
                                </p>
                                <p className="text-default-500">
                                    {expense.description}
                                </p>
                            </div>
                            <div className="flex flex-col items-end">
                                <p>{expense.price}</p>
                                <p className="text-default-500 text-xs">
                                    {currencies?.find((currency: any) => currency.currencyId === expense.currencyId)?.code}
                                </p>
                            </div>
                            <Button isIconOnly variant='light' color='danger' onClick={() => handleDelete(expense.expenseId)}>
                                <MdDelete />
                            </Button>
                        </div>
                    ))}
                </div>
                <CardFooter className='flex flex-col items-start'>
                <Select
                        label="Currency"
                        size='sm'
                        defaultSelectedKeys={['USD']}
                        className="max-w-xs mb-4 min-w-[20rem]"
                        onChange={(e) => setSelectedCurrency(e.target.value)}
                    >
                        {currencies?.map((currency: any) => (
                            <SelectItem
                                key={currency.code}
                                value={currency.code}
                                startContent={
                                    <span className="text-default-500 w-8">
                                        {currency.symbol}
                                    </span>
                                }
                            >
                                {currency.name}
                            </SelectItem>
                        ))}
                    </Select>
                    <div className="flex flex-row justify-between w-full">
                        <p className="font-bold text-large">Total:</p>
                        <p>{displayTotal} {selectedCurrency}</p>
                    </div>
                </CardFooter>
                <AddExpenseModal isOpen={isOpen} onOpen={onOpen} onOpenChange={onOpenChange} itineraryId={itineraryId} currencies={currencies} />
            </Card>
        )
    }
);